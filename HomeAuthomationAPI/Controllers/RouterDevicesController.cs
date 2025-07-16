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
    public class RouterDevicesController : BaseController
    {
        public RouterDevicesController(HomeAutomationContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouterDevice>>> Get()
        {
            IQueryable<RouterDevice> query = _context.RouterDevices
                .Include(r => r.Devices)
                .Include(r => r.Configurations)
                .Include(r => r.Property);

            if (!IsGlobalAdmin)
            {
                var user = await GetCurrentUserAsync();
                if (user == null) return Forbid();
                query = query.Where(r => r.Property != null && r.Property.OrganisationId == user.OrganisationId);
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RouterDevice>> Get(int id)
        {
            var router = await _context.RouterDevices
                .Include(r => r.Devices)
                .Include(r => r.Configurations)
                .Include(r => r.Property)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (router == null) return NotFound();

            if (!IsGlobalAdmin)
            {
                var user = await GetCurrentUserAsync();
                if (user == null || router.Property == null || router.Property.OrganisationId != user.OrganisationId)
                    return Forbid();
            }

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
