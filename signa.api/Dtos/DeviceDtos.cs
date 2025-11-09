namespace signa.api.Dtos;

// ==========================================================
// 🔐 REQUESTS
// ==========================================================
public record RegisterDeviceDto(string name);

public record UpdateDeviceDto(string name, string serial_number);

// ==========================================================
// ✅ RESPONSES
// ==========================================================
public record DeviceDto(
    int id,
    string serial_number,
    string? name,
    string status,
    int? battery,
    DateTimeOffset? last_seen_at,
    bool is_active,
    DateTimeOffset created_at,
    DateTimeOffset? updated_at
);
