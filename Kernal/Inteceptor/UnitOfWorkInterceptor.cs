using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SharedKernel;
/// <summary>
/// Intercepts the SaveChanges process in a DbContext to track and log changes to entities that implement the ITrackableEntity interface.
/// It detects modifications to both parent and child entities and logs the changes to an EntityChangeLog table.
/// For each modified entity, it tracks the properties that have changed and logs their old and new values.
/// Changes in child entities are recursively tracked by checking navigation properties and prefixing child entity property names for clarity.
/// The interceptor also records information such as the user making the change, the IP address, and the time of the change for auditing purposes.
/// </summary>
public class UnitOfWorkInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    private static bool _skipInterceptor;

    public static void SkipInterceptor() => _skipInterceptor = true;
    public static void ResumeInterceptor() => _skipInterceptor = false;

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null || _skipInterceptor)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var modifiedEntries = eventData.Context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Modified && e.Entity is ITrackableEntity)
            .ToList();

        List<EntityChangeLog> changeLogs = [];
        List<PropertiesChanges> propsChangeLogs = [];

        string userId = GetUserId() ?? string.Empty;
        string ipAddress = GetUserIPAddress() ?? string.Empty;
        string userName = GetUserName() ?? string.Empty;

        HashSet<ITrackableEntity> processedEntities = [];
        foreach (var entry in modifiedEntries)
        {
            if (processedEntities.Contains((ITrackableEntity)entry.Entity))
                continue;

            var entityName = entry.Entity.GetType().Name;

            TrackEntityChanges(entry, propsChangeLogs);

            foreach (var navigation in entry.Navigations.Where(n => n.CurrentValue is ITrackableEntity))
            {
                if (navigation.CurrentValue is ITrackableEntity childEntity)
                {
                    var childEntry = eventData.Context.Entry(childEntity);
                    TrackEntityChanges(childEntry, propsChangeLogs, isChildEntity: true);
                    processedEntities.Add(childEntity);
                }
            }

            if (propsChangeLogs.Count > 0)
            {
                changeLogs.Add(new EntityChangeLog
                {
                    EntityId = long.TryParse(entry.CurrentValues["Id"]?.ToString(), out var id) ? id : 0,
                    EntityName = entityName,
                    PropertiesChanges = propsChangeLogs.SerializeJson(),
                    ChangeTime = DateTimeOffset.UtcNow,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ChangeType = entry.State.ToString(),
                    UserId = userId ?? string.Empty,
                    IPAddress = ipAddress ?? string.Empty,
                    UserName = userName ?? string.Empty,
                    StatusId = (short)EntityStatus.Active
                });

                propsChangeLogs = [];
            }
        }

        if (changeLogs.Count > 0)
            await eventData.Context.Set<EntityChangeLog>().AddRangeAsync(changeLogs, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private string? GetUserId() => httpContextAccessor.HttpContext?.User?.FindFirst(PropertyConstants.Id)?.Value;
    private string? GetUserIPAddress() => httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
    private string? GetUserName() => httpContextAccessor.HttpContext?.User?.FindFirst(PropertyConstants.UserName)?.Value;


    private List<PropertiesChanges> TrackEntityChanges(EntityEntry entry, List<PropertiesChanges> propsChangeLogs, bool isChildEntity = false)
    {
        var trackableProperties = ( (ITrackableEntity)entry.Entity ).GetTrackableProperties();

        foreach (var propName in trackableProperties)
        {
            var originalValue = entry.OriginalValues[propName];
            var currentValue = entry.CurrentValues[propName];

            if (Equals(originalValue, currentValue)) continue;

            propsChangeLogs.Add(new PropertiesChanges
            {
                PropertyName = isChildEntity ? $"{entry.Entity.GetType().Name}.{propName}" : propName,
                OldValue = originalValue?.ToString() ?? string.Empty,
                NewValue = currentValue?.ToString() ?? string.Empty,
            });
        }
        return propsChangeLogs;
    }
}
