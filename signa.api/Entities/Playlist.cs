using signa.api.Entities.Common;

namespace Signa.Api.Entities;

public class Playlist : SoftBase
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // FK
    public int? CreatedBy { get; set; }
    public User? User { get; set; }

    // Navigation
    public ICollection<PlaylistMedia>? PlaylistMedia { get; set; }
    public ICollection<DevicePlaylist>? DevicePlaylists { get; set; }
}