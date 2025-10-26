using System.ComponentModel.DataAnnotations.Schema;

namespace signa.api.Entities.Common;

public abstract class SoftBase
{
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    [Column("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
    [Column("deleted_at")]
    public DateTimeOffset? DeletedAt { get; set; }
}