using System.ComponentModel.DataAnnotations.Schema;

namespace Signa.Api.Entities;

[Table("device_playlists")]
public class DevicePlaylist
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("device_id")]
    public int DeviceId { get; set; }
    
    [Column("playlist_id")]
    public int PlaylistId { get; set; }
    
    [Column("assigned_at")]
    public DateTimeOffset? AssignedAt { get; set; }

    // Navigation
    public Device Device { get; set; } = null!;
    public Playlist Playlist { get; set; } = null!;
}