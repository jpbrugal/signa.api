using Microsoft.EntityFrameworkCore;
using Signa.Api.Data;
using Signa.Api.Entities;
using signa.api.Dtos;
using signa.api.Models;

namespace signa.api.Services.Devices;

public class DeviceService : IDeviceService
{
    private readonly AppDbContext _db;

    public DeviceService(AppDbContext db)
    {
        _db = db;
    }

    // ==========================================================
    //  REGISTER DEVICE
    // ==========================================================
    public async Task<ServiceResponse<DeviceDto>> RegisterDeviceAsync(RegisterDeviceDto dto)
    {
        // Generate random GUID as serial number
        var serialNumber = Guid.NewGuid().ToString().ToUpperInvariant();

        var device = new Device
        {
            SerialNumber = serialNumber,
            Name = dto.name,
            Status = "offline",
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _db.Devices.Add(device);
        await _db.SaveChangesAsync();

        var deviceDto = MapToDto(device);
        return ServiceResponse<DeviceDto>.Ok(deviceDto, "Device registered successfully.");
    }

    // ==========================================================
    //  GET DEVICES (with optional pagination, search, id, serialNumber)
    // ==========================================================
    public async Task<ServiceResponse<List<DeviceDto>>> GetDevicesAsync(
        int? id = null,
        string? search = null,
        string? serialNumber = null,
        int? page = null,
        int? pageSize = null)
    {
        // Start with base query - only non-deleted devices
        var query = _db.Devices
            .Where(d => d.DeletedAt == null)
            .AsQueryable();

        // Filter by ID if provided
        if (id.HasValue)
        {
            query = query.Where(d => d.Id == id.Value);
        }

        // Filter by serial number if provided
        if (!string.IsNullOrWhiteSpace(serialNumber))
        {
            query = query.Where(d => d.SerialNumber == serialNumber);
        }

        // Search by name or serial number if provided
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(d => 
                (d.Name != null && d.Name.Contains(search)) || 
                d.SerialNumber.Contains(search));
        }

        // Order by creation date (newest first)
        query = query.OrderByDescending(d => d.CreatedAt);

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
        var devices = await query
            .Select(d => new DeviceDto(
                d.Id,
                d.SerialNumber,
                d.Name,
                d.Status,
                d.Battery,
                d.LastSeenAt,
                d.IsActive,
                d.CreatedAt,
                d.UpdatedAt
            ))
            .ToListAsync();

        return ServiceResponse<List<DeviceDto>>.Ok(
            devices,
            "Devices retrieved successfully.",
            pagination
        );
    }

    // ==========================================================
    //  UPDATE DEVICE (name and serial number)
    // ==========================================================
    public async Task<ServiceResponse<DeviceDto>> UpdateDeviceAsync(int deviceId, UpdateDeviceDto dto)
    {
        // Find the device
        var device = await _db.Devices
            .FirstOrDefaultAsync(d => d.Id == deviceId && d.DeletedAt == null);

        if (device is null)
            return ServiceResponse<DeviceDto>.Fail("Device not found.");

        // Check if serial number is already taken by another device
        var serialNumberExists = await _db.Devices
            .AnyAsync(d => d.SerialNumber == dto.serial_number && d.Id != deviceId && d.DeletedAt == null);

        if (serialNumberExists)
            return ServiceResponse<DeviceDto>.Fail("Serial number is already in use by another device.");

        // Update name and serial number
        device.Name = dto.name;
        device.SerialNumber = dto.serial_number;
        device.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();

        var deviceDto = MapToDto(device);
        return ServiceResponse<DeviceDto>.Ok(deviceDto, "Device updated successfully.");
    }

    // ==========================================================
    //  TOGGLE DEVICE ACTIVE STATUS
    // ==========================================================
    public async Task<ServiceResponse<DeviceDto>> ToggleDeviceActiveAsync(int deviceId)
    {
        // Find the device
        var device = await _db.Devices
            .FirstOrDefaultAsync(d => d.Id == deviceId && d.DeletedAt == null);

        if (device is null)
            return ServiceResponse<DeviceDto>.Fail("Device not found.");

        // Toggle the IsActive status
        device.IsActive = !device.IsActive;
        device.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();

        var deviceDto = MapToDto(device);
        var message = device.IsActive
            ? "Device activated successfully."
            : "Device deactivated successfully.";

        return ServiceResponse<DeviceDto>.Ok(deviceDto, message);
    }

    // ==========================================================
    // ️ SOFT DELETE DEVICE
    // ==========================================================
    public async Task<ServiceResponse<bool>> DeleteDeviceAsync(int deviceId)
    {
        // Find the device
        var device = await _db.Devices
            .FirstOrDefaultAsync(d => d.Id == deviceId && d.DeletedAt == null);

        if (device is null)
            return ServiceResponse<bool>.Fail("Device not found.");

        // Soft delete by setting DeletedAt
        device.DeletedAt = DateTimeOffset.UtcNow;
        device.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();

        return ServiceResponse<bool>.Ok(true, "Device deleted successfully.");
    }

    // ==========================================================
    // 🔧 HELPER METHODS
    // ==========================================================

    private static DeviceDto MapToDto(Device device)
    {
        return new DeviceDto(
            device.Id,
            device.SerialNumber,
            device.Name,
            device.Status,
            device.Battery,
            device.LastSeenAt,
            device.IsActive,
            device.CreatedAt,
            device.UpdatedAt
        );
    }
}
