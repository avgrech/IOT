using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeAuthomationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationsController : ControllerBase
    {
        private readonly HomeAutomationContext _context;
        public ConfigurationsController(HomeAutomationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Configuration>>> Get()
        {
            return await _context.Configurations.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Configuration>> Get(int id)
        {
            var config = await _context.Configurations.FindAsync(id);
            if (config == null) return NotFound();
            return config;
        }

        [HttpPost]
        public async Task<ActionResult<Configuration>> Post(Configuration config)
        {
            _context.Configurations.Add(config);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = config.Id }, config);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Configuration config)
        {
            if (id != config.Id) return BadRequest();
            _context.Entry(config).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var config = await _context.Configurations.FindAsync(id);
            if (config == null) return NotFound();
            _context.Configurations.Remove(config);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
