using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HomeAuthomationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly HomeAutomationContext _context;
        public SyncController(HomeAutomationContext context)
        {
            _context = context;
        }

        public class SyncRequest
        {
            public string RouterDeviceId { get; set; } = string.Empty;
            public Dictionary<string, Dictionary<string, object>> DeviceStatuses { get; set; } = new();
        }

        public class SyncResponse
        {
            public string? ConfigurationContent { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult<SyncResponse>> Post(SyncRequest request)
        {
            var router = await _context.RouterDevices
                .Include(r => r.Property)
                    .ThenInclude(p => p!.Configuration)
                .Include(r => r.Devices)
                .FirstOrDefaultAsync(r => r.UniqueId == request.RouterDeviceId);

            if (router != null)
            {
                foreach (var kvp in request.DeviceStatuses)
                {
                    var device = router.Devices.FirstOrDefault(d => d.Name == kvp.Key);
                    if (device == null) continue;

                    var ds = new DeviceStatus
                    {
                        DeviceId = device.Id,
                        Timestamp = DateTime.UtcNow,
                        StatusJson = JsonSerializer.Serialize(kvp.Value)
                    };
                    _context.DeviceStatuses.Add(ds);
                }
                await _context.SaveChangesAsync();
            }

            string? config = router?.Property?.Configuration?.Content;

            return Ok(new SyncResponse { ConfigurationContent = config });
        }
    }
}
