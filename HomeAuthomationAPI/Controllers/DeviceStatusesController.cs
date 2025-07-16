using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeAuthomationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceStatusesController : ControllerBase
{
    private readonly HomeAutomationContext _context;

    public DeviceStatusesController(HomeAutomationContext context)
    {
        _context = context;
    }

    [HttpGet("latest/{routerDeviceId}")]
    public async Task<ActionResult<DeviceStatus>> GetLatest(string routerDeviceId)
    {
        var status = await _context.DeviceStatuses
            .Where(s => s.RouterDeviceId == routerDeviceId)
            .OrderByDescending(s => s.Timestamp)
            .FirstOrDefaultAsync();
        if (status == null) return NotFound();
        return status;
    }
}
