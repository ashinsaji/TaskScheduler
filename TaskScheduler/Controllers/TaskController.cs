using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskScheduler.Dto;
using TaskScheduler.Models;
using TaskScheduler.Repositories;
using Task = System.Threading.Tasks;

namespace TaskScheduler.Controllers;

[Route("api/v1/tasks")]
[ApiController]
public class TaskController(TaskSchedulerDbContext context) : ControllerBase
{

    private readonly TaskSchedulerDbContext _context = context;

    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var tasks = await _context.Tasks
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Name = t.Name,
                Procedure = t.Procedure,
                SopId = t.SopId
            })
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await _context.Tasks
            .Where(t => t.Id == id)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Name = t.Name,
                Procedure = t.Procedure,
                SopId = t.SopId
            })
            .FirstOrDefaultAsync();

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetTasks([FromQuery] string? namePrefix, [FromQuery] int? sopId)
    {
        var query = _context.Tasks.Include(t => t.Sop).AsQueryable();

        if (!string.IsNullOrEmpty(namePrefix))
        {
            query = query.Where(t => t.Name.StartsWith(namePrefix));
        }

        if (sopId.HasValue)
        {
            query = query.Where(t => t.SopId == sopId.Value);
        }

        var tasks = await query.Select(t => new TaskDto
        {
            Id = t.Id,
            Name = t.Name,
            Procedure = t.Procedure,
            SopId = t.SopId
        }).ToListAsync();

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var sopExists = await _context.Sops.AnyAsync(s => s.Id == createDto.SopId);
        if (!sopExists)
        {
            return BadRequest("Invalid SOP ID.");
        }

        var task = new Models.Task
        {
            Name = createDto.Name,
            Procedure = createDto.Procedure,
            SopId = createDto.SopId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, new TaskDto
        {
            Id = task.Id,
            Name = task.Name,
            Procedure = task.Procedure,
            SopId = task.SopId
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingTask = await _context.Tasks.FindAsync(id);
        if (existingTask == null)
        {
            return NotFound();
        }

        var sopExists = await _context.Sops.AnyAsync(s => s.Id == updateDto.SopId);
        if (!sopExists)
        {
            return BadRequest("Invalid SOP ID.");
        }

        existingTask.Name = updateDto.Name;
        existingTask.Procedure = updateDto.Procedure;
        existingTask.SopId = updateDto.SopId;

        _context.Tasks.Update(existingTask);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}