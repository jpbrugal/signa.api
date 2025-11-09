using Microsoft.EntityFrameworkCore;
using Signa.Api.Data;
using signa.api.Dtos;
using Signa.Api.Entities;
using signa.api.Helpers;
using signa.api.Helpers.Token;
using signa.api.Models;

namespace signa.api.Services.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokens;
    private readonly int _refreshDays;

    public AuthService(AppDbContext db, ITokenService tokens, IConfiguration cfg)
    {
        _db = db;
        _tokens = tokens;
        _refreshDays = int.Parse(cfg["Jwt:RefreshTokenDays"] ?? "14");
    }

    // ==========================================================
    //  REGISTER
    // ==========================================================
    public async Task<ServiceResponse<UserResponseDto>> RegisterAsync(RegisterRequestDto dto)
    {
        // Check if user exists
        if (await _db.Users.AnyAsync(u => u.Email == dto.email))
            return ServiceResponse<UserResponseDto>.Fail("Email is already registered.");

        var user = new User
        {
            Email = dto.email,
            PasswordHash = PasswordHasher.HashPassword(dto.password),
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var userDto = new UserResponseDto(user.Id, user.Email, user.IsActive);
        return ServiceResponse<UserResponseDto>.Ok(userDto, "User registered successfully.");
    }

    // ==========================================================
    // LOGIN
    // ==========================================================
    public async Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginRequestDto dto, string? ip)
    {
        var user = await _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == dto.email && u.IsActive);

        if (user is null || !PasswordHasher.VerifyPassword(dto.password, user.PasswordHash))
            return ServiceResponse<AuthResponseDto>.Fail("Email or password is incorrect.");

        // Remove existing refresh tokens (1 per user)
        var oldTokens = _db.RefreshTokens.Where(t => t.UserId == user.Id);
        _db.RefreshTokens.RemoveRange(oldTokens);
        await _db.SaveChangesAsync();

        // Generate tokens
        var accessToken = _tokens.GenerateAccessToken(user);
        var refreshTokenValue = _tokens.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(_refreshDays),
            CreatedAt = DateTimeOffset.UtcNow
        };

        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();

        var expiresSeconds = (int)TimeSpan.FromMinutes(15).TotalSeconds;

        var response = new AuthResponseDto(
            user.Id,
            user.Email,
            accessToken,
            refreshTokenValue,
            "Bearer",
            expiresSeconds
        );

        return ServiceResponse<AuthResponseDto>.Ok(response, "Login successful.");
    }

    // ==========================================================
    //  REFRESH TOKEN
    // ==========================================================
    public async Task<ServiceResponse<AuthResponseDto>> RefreshAsync(RefreshRequestDto dto, string? ip)
    {
        var principal = _tokens.GetPrincipalFromExpiredToken(dto.access_token);
        if (principal == null)
            return ServiceResponse<AuthResponseDto>.Fail("Token invalid or expired.");

        var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return ServiceResponse<AuthResponseDto>.Fail("Invalid token.");

        var token = await _db.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == dto.refresh_token && t.UserId == userId);

        if (token is null || !token.IsActive)
            return ServiceResponse<AuthResponseDto>.Fail("Refresh token invalid or expired.");

        // Replace the old refresh token
        _db.RefreshTokens.Remove(token);

        var newRefreshValue = _tokens.GenerateRefreshToken();
        var newToken = new RefreshToken
        {
            UserId = userId,
            Token = newRefreshValue,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(_refreshDays),
            CreatedAt = DateTimeOffset.UtcNow
        };
        _db.RefreshTokens.Add(newToken);

        var user = await _db.Users.AsNoTracking().FirstAsync(u => u.Id == userId);
        var newAccess = _tokens.GenerateAccessToken(user);

        await _db.SaveChangesAsync();

        var expiresSeconds = (int)TimeSpan.FromMinutes(15).TotalSeconds;
        var response = new AuthResponseDto(user.Id, user.Email, newAccess, newRefreshValue, "Bearer", expiresSeconds);

        return ServiceResponse<AuthResponseDto>.Ok(response, "Token refreshed successfully.");
    }

    // ==========================================================
    //  REVOKE TOKEN
    // ==========================================================
    public async Task<ServiceResponse<bool>> RevokeAsync(string refreshToken, string? ip)
    {
        var token = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
        if (token is null || !token.IsActive)
            return ServiceResponse<bool>.Fail("Refresh token not found or expired.");

        _db.RefreshTokens.Remove(token);
        await _db.SaveChangesAsync();

        return ServiceResponse<bool>.Ok(true, "Token revoked successfully.");
    }
}