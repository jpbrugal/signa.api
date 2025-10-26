using signa.api.Entities.Common;

namespace Signa.Api.Entities;

public class User : SoftBase
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Media>? Media { get; set; }
    public ICollection<Playlist>? Playlists { get; set; }
}