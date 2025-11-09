using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.api.Dtos;
using signa.api.Services.Users;

namespace signa.api.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int? id = null,
        [FromQuery] string? search = null,
        [FromQuery] int? page = null,
        [FromQuery(Name = "page_size")] int? pageSize = null)
    {
        var result = await _userService.GetUsersAsync(id, search, page, pageSize);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmail(int id, [FromBody] UpdateUserEmailDto dto)
    {
        var result = await _userService.UpdateUserEmailAsync(id, dto);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    [HttpPatch("{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var result = await _userService.ToggleUserActiveAsync(id);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}

