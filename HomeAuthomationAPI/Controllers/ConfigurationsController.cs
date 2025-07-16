using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeAuthomationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConfigurationsController : BaseController
    {
        public ConfigurationsController(HomeAutomationContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Configuration>>> Get()
        {
            IQueryable<Configuration> query = _context.Configurations
                .Include(c => c.Property)
                .Include(c => c.RouterDevice)
                .Include(c => c.Device)
                    .ThenInclude(d => d.RouterDevice)
                        .ThenInclude(r => r.Property);

            if (!IsGlobalAdmin)
            {
                var user = await GetCurrentUserAsync();
                if (user == null) return Forbid();
                query = query.Where(c =>
                    (c.Property != null && c.Property.OrganisationId == user.OrganisationId) ||
                    (c.RouterDevice != null && c.RouterDevice.Property!.OrganisationId == user.OrganisationId) ||
                    (c.Device != null && c.Device.RouterDevice!.Property!.OrganisationId == user.OrganisationId));
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Configuration>> Get(int id)
        {
            var config = await _context.Configurations
                .Include(c => c.Property)
                .Include(c => c.RouterDevice)
                .Include(c => c.Device)
                    .ThenInclude(d => d.RouterDevice)
                        .ThenInclude(r => r.Property)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (config == null) return NotFound();

            if (!IsGlobalAdmin)
            {
                var user = await GetCurrentUserAsync();
                if (user == null ||
                    !((config.Property != null && config.Property.OrganisationId == user.OrganisationId) ||
                      (config.RouterDevice != null && config.RouterDevice.Property!.OrganisationId == user.OrganisationId) ||
                      (config.Device != null && config.Device.RouterDevice!.Property!.OrganisationId == user.OrganisationId)))
                {
                    return Forbid();
                }
            }

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
