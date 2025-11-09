using signa.api.Dtos;
using signa.api.Models;

namespace signa.api.Services.Users;

public interface IUserService
{
    Task<ServiceResponse<List<UserDto>>> GetUsersAsync(int? id = null, string? search = null, int? page = null, int? pageSize = null);
    Task<ServiceResponse<UserDto>> UpdateUserEmailAsync(int userId, UpdateUserEmailDto dto);
    Task<ServiceResponse<UserDto>> ToggleUserActiveAsync(int userId);
    Task<ServiceResponse<bool>> DeleteUserAsync(int userId);
}

