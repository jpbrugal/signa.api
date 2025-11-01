using System.Security.Claims;
using Signa.Api.Entities;

namespace signa.api.Helpers.Token;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}