namespace SharedKernel;

public class EntityDeletedEvent<T>(T entity, DateTime eventDateTime) : IDomainEvent
        where T : BaseEntity
{
    public T Entity { get; } = entity;
    public DateTime EventDateTime { get; } = eventDateTime;
}
