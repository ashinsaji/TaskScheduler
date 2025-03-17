using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskScheduler.Dto;
using TaskScheduler.Models;
using TaskScheduler.Repositories;

namespace TaskScheduler.Controllers;

[Route("api/employeetasks")]
[ApiController]
public class EmployeeTasksController(TaskSchedulerDbContext context) : ControllerBase
{
    private readonly TaskSchedulerDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAllEmployeeTasks()
    {
        var employeeTasks = await _context.EmployeeTasks
            .Include(et => et.Employee)
            .Include(et => et.Task)
            .Select(et => new EmployeeTaskDto
            {
                EmployeeId = et.EmployeeId,
                EmployeeName = et.Employee.Name,
                TaskId = et.TaskId,
                TaskName = et.Task.Name,
                PriorityCode = et.PriorityCode
            })
            .ToListAsync();

        return Ok(employeeTasks);
    }
    
    [HttpGet("{taskId}/{employeeId}")]
    public async Task<IActionResult> GetEmployeeTask(int employeeId, int taskId)
    {
        var employeeTask = await _context.EmployeeTasks
            .Where(et => et.EmployeeId == employeeId && et.TaskId == taskId)
            .Select(et => new
            {
                et.EmployeeId,
                et.TaskId,
                et.PriorityCode
            })
            .FirstOrDefaultAsync();

        if (employeeTask == null)
        {
            return NotFound();
        }

        return Ok(employeeTask);
    }
    
    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetEmployeeTasksByTaskId(int taskId)
    {
        var employeeTasks = await _context.EmployeeTasks
            .Where(et => et.TaskId == taskId)
            .Include(et => et.Employee) // Include Employee details
            .Select(et => new
            {
                et.EmployeeId,
                EmployeeName = et.Employee.Name,
                et.TaskId,
                et.PriorityCode
            })
            .ToListAsync();

        if (!employeeTasks.Any())
        {
            return NotFound($"No employees assigned to task with ID {taskId}.");
        }

        return Ok(employeeTasks);
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployeeTask([FromBody] CreateEmployeeTaskDto? createDto)
    {
        if (createDto == null)
        {
            return BadRequest("Invalid data.");
        }

        var employeeExists = await _context.Employees.AnyAsync(e => e.Id == createDto.EmployeeId);
        var taskExists = await _context.Tasks.AnyAsync(t => t.Id == createDto.TaskId);

        if (!employeeExists || !taskExists)
        {
            return BadRequest("Invalid EmployeeId or TaskId.");
        }
        
        var employeeTask = await _context.EmployeeTasks
            .FirstOrDefaultAsync(et => et.EmployeeId == createDto.EmployeeId && et.TaskId == createDto.TaskId);

        if (employeeTask is not null)
        {
            employeeTask.PriorityCode = createDto.PriorityCode;

            _context.EmployeeTasks.Update(employeeTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        employeeTask = new EmployeeTask
        {
            EmployeeId = createDto.EmployeeId,
            TaskId = createDto.TaskId,
            PriorityCode = createDto.PriorityCode
        };
        
        _context.EmployeeTasks.Add(employeeTask);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployeeTask), new { createDto.EmployeeId, createDto.TaskId }, createDto);
    }

    [HttpPut("{employeeId}/{taskId}")]
    public async Task<IActionResult> UpdateEmployeeTask(int employeeId, int taskId, [FromBody] UpdateEmployeeTaskDto? updateDto)
    {
        if (updateDto == null)
        {
            return BadRequest("Invalid data.");
        }

        var existingEmployeeTask = await _context.EmployeeTasks
            .FirstOrDefaultAsync(et => et.EmployeeId == employeeId && et.TaskId == taskId);

        if (existingEmployeeTask == null)
        {
            return NotFound();
        }

        existingEmployeeTask.PriorityCode = updateDto.PriorityCode;

        _context.EmployeeTasks.Update(existingEmployeeTask);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{employeeId}/{taskId}")]
    public async Task<IActionResult> DeleteEmployeeTask(int employeeId, int taskId)
    {
        var employeeTask = await _context.EmployeeTasks
            .FirstOrDefaultAsync(et => et.EmployeeId == employeeId && et.TaskId == taskId);

        if (employeeTask == null)
        {
            return NotFound();
        }

        _context.EmployeeTasks.Remove(employeeTask);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
