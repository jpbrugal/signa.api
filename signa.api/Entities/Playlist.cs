using System.ComponentModel.DataAnnotations.Schema;
using signa.api.Entities.Common;

namespace Signa.Api.Entities;

[Table("playlists")]
public class Playlist : SoftBase
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; } = null!;
    
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; } = false;

    // FK
    [Column("created_by")]
    public int? CreatedBy { get; set; }
    public User? User { get; set; }

    // Navigation
    public ICollection<PlaylistMedia>? PlaylistMedia { get; set; }
    public ICollection<DevicePlaylist>? DevicePlaylists { get; set; }
}