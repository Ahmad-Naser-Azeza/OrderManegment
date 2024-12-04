namespace SharedKernel;

public class EntityUpdatedWithChngesEvent<T>(T entity, string Changes, DateTime eventDateTime) : IDomainEvent
           where T : BaseEntity
    {
        public T Entity { get; } = entity;
        public string Changes { get; set; } = Changes;
        public DateTime EventDateTime { get; } = eventDateTime;
    }
