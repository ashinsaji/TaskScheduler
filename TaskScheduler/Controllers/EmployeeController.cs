using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskScheduler.Dto;
using TaskScheduler.Models;
using TaskScheduler.Repositories;

namespace TaskScheduler.Controllers;

[Route("api/v1/employees")]
[ApiController]
public class EmployeeController(TaskSchedulerDbContext context) : ControllerBase
{
    private readonly TaskSchedulerDbContext _context = context;
    
    [HttpGet]
    public async Task<ActionResult<List<Employee>>> GetAllEmployees()
    {
        var employees = await _context.Employees
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                DepartmentId = e.DepartmentId
            })
            .ToListAsync();

        return Ok(employees);
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<List<Employee>>> GetEmployeeByName([FromQuery] string? namePrefix, [FromQuery] int? departmentId)
    {
        var query = _context.Employees.Include(e => e.Department).AsQueryable();
        if (!string.IsNullOrEmpty(namePrefix))
        {
            query = query.Where(e => e.Name.StartsWith(namePrefix));
        }

        if (departmentId.HasValue)
        {
            query = query.Where(e => e.DepartmentId == departmentId.Value);
        }

        var employees = await query
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                DepartmentId = e.DepartmentId
            })
            .ToListAsync();

        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployeeById(int id)
    {
        var employee = await _context.Employees
            .Where(e => e.Id == id)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                DepartmentId = e.DepartmentId
            })
            .FirstOrDefaultAsync();

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee([FromBody] CreateEmployeeDto createDto)
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

        var employee = new Employee
        {
            Name = createDto.Name,
            DepartmentId = createDto.DepartmentId
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, new EmployeeDto
        {
            Id = employee.Id,
            Name = employee.Name,
            DepartmentId = employee.DepartmentId
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingEmployee = await _context.Employees.FindAsync(id);
        if (existingEmployee == null)
        {
            return NotFound();
        }

        var departmentExists = await _context.Departments.AnyAsync(d => d.Id == updateDto.DepartmentId);
        if (!departmentExists)
        {
            return BadRequest("Invalid Department ID.");
        }

        existingEmployee.Name = updateDto.Name;
        existingEmployee.DepartmentId = updateDto.DepartmentId;

        _context.Employees.Update(existingEmployee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee is null)
            return NotFound();
        
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
