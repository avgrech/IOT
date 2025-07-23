using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeAuthomationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParametersController : BaseController
{
    public ParametersController(HomeAutomationContext context) : base(context)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Parameter>>> Get()
    {
        return await _context.Parameters
            .Include(p => p.DeviceType)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Parameter>> Get(int id)
    {
        var parameter = await _context.Parameters
            .Include(p => p.DeviceType)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (parameter == null) return NotFound();
        return parameter;
    }

    [HttpPost]
    public async Task<ActionResult<Parameter>> Post(Parameter parameter)
    {
        _context.Parameters.Add(parameter);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = parameter.Id }, parameter);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Parameter parameter)
    {
        if (id != parameter.Id) return BadRequest();
        _context.Entry(parameter).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var parameter = await _context.Parameters.FindAsync(id);
        if (parameter == null) return NotFound();
        _context.Parameters.Remove(parameter);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
