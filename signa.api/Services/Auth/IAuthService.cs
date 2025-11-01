using signa.api.Dtos;
using signa.api.Models;

namespace signa.api.Services.Auth;

public interface IAuthService
{
    Task<ServiceResponse<UserResponseDto>> RegisterAsync(RegisterRequestDto dto);
    Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginRequestDto dto, string? ip);
    Task<ServiceResponse<AuthResponseDto>> RefreshAsync(RefreshRequestDto dto, string? ip);
    Task<ServiceResponse<bool>> RevokeAsync(string refreshToken, string? ip);
}