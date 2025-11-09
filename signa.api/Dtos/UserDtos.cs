namespace signa.api.Dtos;

// ==========================================================
// 🔐 REQUESTS
// ==========================================================
public record UpdateUserEmailDto(string email);

// ==========================================================
// ✅ RESPONSES
// ==========================================================
public record UserDto(
    int id,
    string email,
    bool is_active,
    DateTimeOffset created_at,
    DateTimeOffset? updated_at
);

