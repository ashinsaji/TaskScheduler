using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskScheduler.Models;

public class Sop
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }
    
    public int DepartmentId { get; set; } 
    public Department? Department { get; set; }

    public List<Task>? Tasks { get; set; }
}