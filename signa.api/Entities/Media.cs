using signa.api.Entities.Common;

namespace Signa.Api.Entities;

public class Media : SoftBase
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!; // image | video | html
    public string Url { get; set; } = null!;
    public long? SizeBytes { get; set; }
    public int? DurationSec { get; set; }

    // FK
    public int? UploadedBy { get; set; }
    public User? User { get; set; }

    // Navigation
    public ICollection<PlaylistMedia>? PlaylistMedia { get; set; }
    
}