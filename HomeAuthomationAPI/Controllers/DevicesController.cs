using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeAuthomationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly HomeAutomationContext _context;
        public DevicesController(HomeAutomationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> Get()
        {
            return await _context.Devices
                .Include(d => d.Parameters)
                .Include(d => d.Configurations)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> Get(int id)
        {
            var device = await _context.Devices
                .Include(d => d.Parameters)
                .Include(d => d.Configurations)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (device == null) return NotFound();
            return device;
        }

        [HttpPost]
        public async Task<ActionResult<Device>> Post(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = device.Id }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Device device)
        {
            if (id != device.Id) return BadRequest();
            _context.Entry(device).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound();
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
