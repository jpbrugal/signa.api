using System.ComponentModel.DataAnnotations.Schema;
using signa.api.Entities.Common;

namespace Signa.Api.Entities;

[Table("devices")]
public class Device : SoftBase
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("identifier")]
    public string Identifier { get; set; } = null!;
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("status")]
    public string Status { get; set; } = "offline"; // online | offline
    
    [Column("battery")]
    public int? Battery { get; set; }               // snapshot of latest battery
    
    [Column("last_seen_at")]
    public DateTimeOffset? LastSeenAt { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; } = false;

    // Navigation
    public ICollection<DeviceHeartbeat>? Heartbeats { get; set; }
    public ICollection<DevicePlaylist>? DevicePlaylists { get; set; }
}