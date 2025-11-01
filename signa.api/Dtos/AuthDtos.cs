namespace signa.api.Dtos;

// ==========================================================
// 🔐 REQUESTS
// ==========================================================
public record RegisterRequestDto(string email, string password);

public record LoginRequestDto(string email, string password);

public record RefreshRequestDto(string access_token, string refresh_token);

// ==========================================================
// ✅ RESPONSES
// ==========================================================
public record AuthResponseDto(
    int user_id,
    string email,
    string access_token,
    string refresh_token,
    string token_type,
    int expires_in_seconds
);

public record UserResponseDto(
    int id,
    string email,
    bool is_active
);