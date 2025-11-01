using System.ComponentModel.DataAnnotations.Schema;

namespace Signa.Api.Entities;

[Table("refresh_tokens")]
public class RefreshToken
{
    [Column("id")]
    public long Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("token")]
    public string Token { get; set; } = null!;

    [Column("expires_at")]
    public DateTimeOffset ExpiresAt { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("revoked_at")]
    public DateTimeOffset? RevokedAt { get; set; }

    public bool IsActive => RevokedAt == null && DateTimeOffset.UtcNow < ExpiresAt;

    // Navigation
    public User User { get; set; } = null!;
}