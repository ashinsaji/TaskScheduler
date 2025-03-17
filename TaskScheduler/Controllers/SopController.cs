using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskScheduler.Dto;
using TaskScheduler.Models;
using TaskScheduler.Repositories;

namespace TaskScheduler.Controllers;

[Route("api/v1/sops")]
[ApiController]
public class SopController(TaskSchedulerDbContext context) : ControllerBase
{
    private readonly TaskSchedulerDbContext _context = context;
    
    [HttpGet]
    public async Task<ActionResult<List<Sop>>> GetAllSops()
    {
        var sops = await _context.Sops
            .Select(s => new SopDto
            {
                Id = s.Id,
                Name = s.Name,
                DepartmentId = s.DepartmentId
            })
            .ToListAsync();

        return Ok(sops);
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> GetSops([FromQuery] string? namePrefix, [FromQuery] int? departmentId)
    {
        var query = _context.Sops.Include(s => s.Department).AsQueryable();

        if (!string.IsNullOrEmpty(namePrefix))
        {
            query = query.Where(s => s.Name.StartsWith(namePrefix));
        }

        if (departmentId.HasValue)
        {
            query = query.Where(s => s.DepartmentId == departmentId.Value);
        }

        var sops = await query
            .Select(s => new SopDto
            {
                Id = s.Id,
                Name = s.Name,
                DepartmentId = s.DepartmentId
            })
            .ToListAsync();

        return Ok(sops);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sop>> GetSopById(int id)
    {
        var sop = await _context.Sops
            .Where(s => s.Id == id)
            .Select(s => new SopDto
            {
                Id = s.Id,
                Name = s.Name,
                DepartmentId = s.DepartmentId
            })
            .FirstOrDefaultAsync();

        if (sop == null)
        {
            return NotFound();
        }

        return Ok(sop);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSop([FromBody] CreateSopDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var departmentExists = await _context.Departments.AnyAsync(d => d.Id == createDto.DepartmentId);
        if (!departmentExists)
        {
            return BadRequest("Invalid Department ID.");
        }

        var sop = new Sop
        {
            Name = createDto.Name,
            DepartmentId = createDto.DepartmentId
        };

        _context.Sops.Add(sop);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSopById), new { id = sop.Id }, new SopDto
        {
            Id = sop.Id,
            Name = sop.Name,
            DepartmentId = sop.DepartmentId
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSop(int id, [FromBody] UpdateSopDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingSop = await _context.Sops.FindAsync(id);
        if (existingSop == null)
        {
            return NotFound();
        }

        var departmentExists = await _context.Departments.AnyAsync(d => d.Id == updateDto.DepartmentId);
        if (!departmentExists)
        {
            return BadRequest("Invalid Department ID.");
        }

        existingSop.Name = updateDto.Name;
        existingSop.DepartmentId = updateDto.DepartmentId;

        _context.Sops.Update(existingSop);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSop(int id)
    {
        var sop = await _context.Sops.FindAsync(id);
        if (sop is null)
            return NotFound();
        
        _context.Sops.Remove(sop);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
