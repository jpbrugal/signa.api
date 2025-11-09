using Microsoft.EntityFrameworkCore;
using Signa.Api.Data;
using signa.api.Dtos;
using signa.api.Models;

namespace signa.api.Services.Users;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    // ==========================================================
    //  GET USERS (with optional pagination, search, and id)
    // ==========================================================
    public async Task<ServiceResponse<List<UserDto>>> GetUsersAsync(
        int? id = null,
        string? search = null,
        int? page = null,
        int? pageSize = null)
    {
        // Start with base query - only non-deleted users
        var query = _db.Users
            .Where(u => u.DeletedAt == null)
            .AsQueryable();

        // Filter by ID if provided
        if (id.HasValue)
        {
            query = query.Where(u => u.Id == id.Value);
        }

        // Search by email if provided
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u => u.Email.Contains(search));
        }

        // Order by creation date (newest first)
        query = query.OrderByDescending(u => u.CreatedAt);

        // Get total count for pagination
        var totalItems = await query.CountAsync();

        // Apply pagination if both page and pageSize are provided
        PaginationMetadata? pagination = null;
        if (page.HasValue && pageSize.HasValue)
        {
            var pageNumber = page.Value < 1 ? 1 : page.Value;
            var pageSizeValue = pageSize.Value < 1 ? 10 : pageSize.Value;

            pagination = new PaginationMetadata(pageNumber, pageSizeValue, totalItems);

            query = query
                .Skip((pageNumber - 1) * pageSizeValue)
                .Take(pageSizeValue);
        }

        // Execute query and map to DTOs
        var users = await query
            .Select(u => new UserDto(
                u.Id,
                u.Email,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt
            ))
            .ToListAsync();

        return ServiceResponse<List<UserDto>>.Ok(
            users,
            "Users retrieved successfully.",
            pagination
        );
    }

    // ==========================================================
    // ️ UPDATE USER EMAIL
    // ==========================================================
    public async Task<ServiceResponse<UserDto>> UpdateUserEmailAsync(int userId, UpdateUserEmailDto dto)
    {
        // Find the user
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.DeletedAt == null);

        if (user is null)
            return ServiceResponse<UserDto>.Fail("User not found.");

        // Check if email is already taken by another user
        var emailExists = await _db.Users
            .AnyAsync(u => u.Email == dto.email && u.Id != userId && u.DeletedAt == null);

        if (emailExists)
            return ServiceResponse<UserDto>.Fail("Email is already in use.");

        // Update email
        user.Email = dto.email;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();

        var userDto = new UserDto(
            user.Id,
            user.Email,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt
        );

        return ServiceResponse<UserDto>.Ok(userDto, "Email updated successfully.");
    }

    // ==========================================================
    //  TOGGLE USER ACTIVE STATUS
    // ==========================================================
    public async Task<ServiceResponse<UserDto>> ToggleUserActiveAsync(int userId)
    {
        // Find the user
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.DeletedAt == null);

        if (user is null)
            return ServiceResponse<UserDto>.Fail("User not found.");

        // Toggle the IsActive status
        user.IsActive = !user.IsActive;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();

        var userDto = new UserDto(
            user.Id,
            user.Email,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt
        );

        var message = user.IsActive
            ? "User activated successfully."
            : "User deactivated successfully.";

        return ServiceResponse<UserDto>.Ok(userDto, message);
    }

    // ==========================================================
    //  SOFT DELETE USER
    // ==========================================================
    public async Task<ServiceResponse<bool>> DeleteUserAsync(int userId)
    {
        // Find the user
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.DeletedAt == null);

        if (user is null)
            return ServiceResponse<bool>.Fail("User not found.");

        // Soft delete by setting DeletedAt
        user.DeletedAt = DateTimeOffset.UtcNow;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();

        return ServiceResponse<bool>.Ok(true, "User deleted successfully.");
    }
}

