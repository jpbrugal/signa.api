using signa.api.Dtos;
using signa.api.Models;

namespace signa.api.Services.Devices;

public interface IDeviceService
{
    Task<ServiceResponse<DeviceDto>> RegisterDeviceAsync(RegisterDeviceDto dto);
    Task<ServiceResponse<List<DeviceDto>>> GetDevicesAsync(int? id = null, string? search = null, string? serialNumber = null, int? page = null, int? pageSize = null);
    Task<ServiceResponse<DeviceDto>> UpdateDeviceAsync(int deviceId, UpdateDeviceDto dto);
    Task<ServiceResponse<DeviceDto>> ToggleDeviceActiveAsync(int deviceId);
    Task<ServiceResponse<bool>> DeleteDeviceAsync(int deviceId);
}
