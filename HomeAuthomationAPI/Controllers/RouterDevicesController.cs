using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeAuthomationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouterDevicesController : ControllerBase
    {
        private readonly HomeAutomationContext _context;
        public RouterDevicesController(HomeAutomationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouterDevice>>> Get()
        {
            return await _context.RouterDevices
                .Include(r => r.Devices)
                .Include(r => r.Configurations)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RouterDevice>> Get(int id)
        {
            var router = await _context.RouterDevices
                .Include(r => r.Devices)
                .Include(r => r.Configurations)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (router == null) return NotFound();
            return router;
        }

        [HttpPost]
        public async Task<ActionResult<RouterDevice>> Post(RouterDevice router)
        {
            _context.RouterDevices.Add(router);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = router.Id }, router);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, RouterDevice router)
        {
            if (id != router.Id) return BadRequest();
            _context.Entry(router).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var router = await _context.RouterDevices.FindAsync(id);
            if (router == null) return NotFound();
            _context.RouterDevices.Remove(router);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
