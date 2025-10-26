using System.ComponentModel.DataAnnotations.Schema;
using signa.api.Entities.Common;

namespace Signa.Api.Entities;

[Table("users")]
public class User : SoftBase
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("email")]
    public string Email { get; set; } = null!;
    
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;
    
    [Column("is_active")]
    public bool IsActive { get; set; } = false;

    // Navigation
    public ICollection<Media>? Media { get; set; }
    public ICollection<Playlist>? Playlists { get; set; }
}