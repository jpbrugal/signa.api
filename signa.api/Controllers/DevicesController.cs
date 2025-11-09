using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.api.Dtos;
using signa.api.Services.Devices;

namespace signa.api.Controllers;

[ApiController]
[Route("devices")]
[Authorize]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    // ==========================================================
    // POST api/devices
    // ==========================================================
    [HttpPost]
    public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceDto dto)
    {
        var result = await _deviceService.RegisterDeviceAsync(dto);
        
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetDevices), new { result.Data!.id }, result);
    }

    // ==========================================================
    // GET api/devices?id=1&search=tablet&serialNumber=ABC-123&page=1&pageSize=10
    // ==========================================================
    [HttpGet]
    public async Task<IActionResult> GetDevices(
        [FromQuery] int? id = null,
        [FromQuery] string? search = null,
        [FromQuery(Name = "serial_number")] string? serialNumber = null,
        [FromQuery] int? page = null,
        [FromQuery(Name = "page_size")] int? pageSize = null)
    {
        var result = await _deviceService.GetDevicesAsync(id, search, serialNumber, page, pageSize);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // ==========================================================
    // PUT api/devices/{id}
    // ==========================================================
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDevice(int id, [FromBody] UpdateDeviceDto dto)
    {
        var result = await _deviceService.UpdateDeviceAsync(id, dto);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // ==========================================================
    // PATCH api/devices/{id}/toggle-active
    // ==========================================================
    [HttpPatch("{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var result = await _deviceService.ToggleDeviceActiveAsync(id);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // ==========================================================
    // DELETE api/devices/{id}
    // ==========================================================
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        var result = await _deviceService.DeleteDeviceAsync(id);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
