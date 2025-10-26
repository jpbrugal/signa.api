namespace Signa.Api.Entities;

public class DeviceHeartbeat
{
    public long Id { get; set; }
    public int DeviceId { get; set; }
    public int? Battery { get; set; }
    public string? NetworkStatus { get; set; } // wifi | cellular | none
    public string? IpAddress { get; set; }
    public string? PayloadJson { get; set; }
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation
    public Device Device { get; set; } = null!;
}