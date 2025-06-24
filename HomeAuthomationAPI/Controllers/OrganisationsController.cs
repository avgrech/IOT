using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeAuthomationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganisationsController : ControllerBase
    {
        private readonly HomeAutomationContext _context;
        public OrganisationsController(HomeAutomationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organisation>>> Get()
        {
            return await _context.Organisations
                .Include(o => o.Properties)
                .Include(o => o.Users)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Organisation>> Get(int id)
        {
            var organisation = await _context.Organisations
                .Include(o => o.Properties)
                .Include(o => o.Users)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (organisation == null) return NotFound();
            return organisation;
        }

        [HttpPost]
        public async Task<ActionResult<Organisation>> Post(Organisation organisation)
        {
            _context.Organisations.Add(organisation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = organisation.Id }, organisation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Organisation organisation)
        {
            if (id != organisation.Id) return BadRequest();
            _context.Entry(organisation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var organisation = await _context.Organisations.FindAsync(id);
            if (organisation == null) return NotFound();
            _context.Organisations.Remove(organisation);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
