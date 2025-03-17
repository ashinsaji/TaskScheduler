using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.Models;

public class Task
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string Procedure { get; set; }

    public int SopId { get; set; }
    public Sop? Sop { get; set; }

    public List<EmployeeTask>? EmployeeTasks { get; set; }
}