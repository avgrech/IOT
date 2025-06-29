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
            var status = new DeviceStatus
            {
                RouterDeviceId = request.RouterDeviceId,
                Timestamp = DateTime.UtcNow,
                StatusJson = JsonSerializer.Serialize(request.DeviceStatuses)
            };
            _context.DeviceStatuses.Add(status);
            await _context.SaveChangesAsync();

            var property = await _context.Properties
                .Include(p => p.Configuration)
                .FirstOrDefaultAsync(p => p.RouterDeviceId == request.RouterDeviceId);

            string? config = property?.Configuration?.Content;

            return Ok(new SyncResponse { ConfigurationContent = config });
        }
    }
}
