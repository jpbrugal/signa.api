using System.ComponentModel.DataAnnotations.Schema;
using signa.api.Entities.Common;

namespace Signa.Api.Entities;

[Table("media")]
public class Media : SoftBase
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; } = null!;
    
    [Column("type")]
    public string Type { get; set; } = null!; // image | video | html
    
    [Column("url")]
    public string Url { get; set; } = null!;
    
    [Column("size_bytes")]
    public long? SizeBytes { get; set; }
    
    [Column("duration_sec")]
    public int? DurationSec { get; set; }

    // FK
    [Column("uploaded_by")]
    public int? UploadedBy { get; set; }
    public User? User { get; set; }

    // Navigation
    public ICollection<PlaylistMedia>? PlaylistMedia { get; set; }
    
}