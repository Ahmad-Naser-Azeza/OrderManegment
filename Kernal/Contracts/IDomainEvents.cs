namespace SharedKernel;
public interface IDomainEvents
{
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
