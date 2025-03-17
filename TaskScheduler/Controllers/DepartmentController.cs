using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using TaskScheduler.Dto;
using TaskScheduler.Models;
using TaskScheduler.Repositories;

namespace TaskScheduler.Controllers;

[Route("api/v1/departments")]
[ApiController]
public class DepartmentController(TaskSchedulerDbContext context) : ControllerBase
{
    private readonly TaskSchedulerDbContext _context = context;
    
    [HttpGet]
    public async Task<ActionResult<List<Department>>> GetAllDepartments()
    {
        var departments = await _context.Departments
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name
            })
            .ToListAsync();

        return Ok(departments);    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Department>>> GetDepartmentByName([FromQuery] string prefix)
    {
        var departments = await _context.Departments
            .Where(d => d.Name.StartsWith(prefix))
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name
            })
            .ToListAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> GetDepartmentById(int id)
    {
        var department = await _context.Departments
            .Where(d => d.Id == id)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name
            })
            .FirstOrDefaultAsync();

        if (department == null)
        {
            return NotFound();
        }

        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<Department>> CreateDepartment([FromBody] CreateDepartmentDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var department = new Department
        {
            Name = createDto.Name
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingDepartment = await _context.Departments.FindAsync(id);
        if (existingDepartment == null)
        {
            return NotFound();
        }

        existingDepartment.Name = updateDto.Name;
        _context.Departments.Update(existingDepartment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department is null)
            return NotFound();
        
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}