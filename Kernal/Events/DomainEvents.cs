using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SharedKernel;

public class DomainEvents(IServiceProvider _serviceProvider) : IDomainEvents
{
    private static List<Type> _handlers = [];
   
    public static void RegisterHandlers(Assembly assembly, IServiceCollection services)
    {
        var types = assembly.GetTypes()
                            .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)))
                            .ToList();

        foreach (var type in types)
        {
            services.AddTransient(type);
        }

        _handlers.AddRange(types);
    }

    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        foreach (Type handlerType in _handlers)
        {
            bool canHandleEvent = handlerType.GetInterfaces()
                .Any(x => x.IsGenericType
                    && x.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
                    && x.GenericTypeArguments[0] == domainEvent.GetType());

            if (canHandleEvent)
            {
                dynamic handler = _serviceProvider.GetService(handlerType);
                await handler.HandleAsync((dynamic)domainEvent, cancellationToken);
            }
        }
    }
}
