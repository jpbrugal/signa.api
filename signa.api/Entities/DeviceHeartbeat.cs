using System.ComponentModel.DataAnnotations.Schema;

namespace Signa.Api.Entities;

[Table("device_heartbeats")]
public class DeviceHeartbeat
{
    [Column("id")]
    public long Id { get; set; }
    
    [Column("device_id")]
    public int DeviceId { get; set; }
    
    [Column("battery")]
    public int? Battery { get; set; }
    
    [Column("network_status")]
    public string? NetworkStatus { get; set; } // wifi | cellular | none
    
    [Column("ip_address")]
    public string? IpAddress { get; set; }
    
    [Column("received_at")]
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation
    public Device Device { get; set; } = null!;
}