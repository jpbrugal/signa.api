using System.ComponentModel.DataAnnotations.Schema;
using signa.api.Entities.Common;

namespace Signa.Api.Entities;

[Table("devices")]
public class Device : SoftBase
{
    public int Id { get; set; }
    public string Identifier { get; set; } = null!;
    public string? Name { get; set; }
    public string Status { get; set; } = "offline"; // online | offline
    public int? Battery { get; set; }               // snapshot of latest battery
    public DateTimeOffset? LastSeenAt { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<DeviceHeartbeat>? Heartbeats { get; set; }
    public ICollection<DevicePlaylist>? DevicePlaylists { get; set; }
}