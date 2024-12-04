namespace SharedKernel;
public class EntityChangeLog : BaseEntity
{
    public int Id { get; set; }
    public long EntityId { get; set; }
    public string EntityName { get; set; }
    public string PropertiesChanges { get; set; }
    public DateTimeOffset ChangeTime { get; set; }
    public string ChangeType { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string IPAddress { get; set; }
}

public class EntityChangeLogModel
{
    public int Id { get; set; }
    public long EntityId { get; set; }
    public string EntityName { get; set; }
    public string PropertyName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTimeOffset ChangeTime { get; set; }
    public string ChangeType { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string IPAddress { get; set; }
}

public class PropertiesChanges
{
    public string PropertyName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
}
