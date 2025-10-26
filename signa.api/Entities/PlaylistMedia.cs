using System.ComponentModel.DataAnnotations.Schema;

namespace Signa.Api.Entities;

[Table("playlist_media")]
public class PlaylistMedia
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("playlist_id")]
    public int PlaylistId { get; set; }
    
    [Column("media_id")]
    public int MediaId { get; set; }
    
    [Column("position")]
    public int Position { get; set; }
    
    [Column("duration_sec")]
    public int? DurationSec { get; set; }

    // Navigation
    public Playlist Playlist { get; set; } = null!;
    public Media Media { get; set; } = null!;
}