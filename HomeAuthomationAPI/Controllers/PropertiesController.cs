using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeAuthomationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly HomeAutomationContext _context;
        public PropertiesController(HomeAutomationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> Get()
        {
            return await _context.Properties.Include(p => p.Configuration).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> Get(int id)
        {
            var property = await _context.Properties.Include(p => p.Configuration).FirstOrDefaultAsync(p => p.Id == id);
            if (property == null) return NotFound();
            return property;
        }

        [HttpPost]
        public async Task<ActionResult<Property>> Post(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = property.Id }, property);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Property property)
        {
            if (id != property.Id) return BadRequest();
            _context.Entry(property).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null) return NotFound();
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
