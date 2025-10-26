namespace Signa.Api.Entities;

public class PlaylistMedia
{
    public int Id { get; set; }
    public int PlaylistId { get; set; }
    public int MediaId { get; set; }
    public int Position { get; set; }
    public int? DurationSec { get; set; }

    // Navigation
    public Playlist Playlist { get; set; } = null!;
    public Media Media { get; set; } = null!;
}