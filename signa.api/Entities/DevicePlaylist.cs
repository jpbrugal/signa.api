namespace Signa.Api.Entities;

public class DevicePlaylist
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public int PlaylistId { get; set; }
    public DateTimeOffset? AssignedAt { get; set; }

    // Navigation
    public Device Device { get; set; } = null!;
    public Playlist Playlist { get; set; } = null!;
}