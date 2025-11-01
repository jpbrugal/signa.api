using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.api.Dtos;
using signa.api.Services.Auth;

namespace signa.api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var result = await _auth.RegisterAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await _auth.LoginAsync(dto, ip);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await _auth.RefreshAsync(dto, ip);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("revoke")]
    [Authorize]
    public async Task<IActionResult> Revoke([FromBody] string refresh_token)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await _auth.RevokeAsync(refresh_token, ip);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}