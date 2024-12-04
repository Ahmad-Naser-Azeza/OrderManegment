using System.ComponentModel.DataAnnotations;

namespace SharedKernel;

public abstract class BaseEntity
{
    public long Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public short StatusId { get; set; }

    [MaxLength(50)]
    public string? CreatedBy { get; set; }

    [MaxLength(50)]
    public string? UpdatedBy { get; set; }
}


