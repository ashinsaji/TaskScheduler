using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskScheduler.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }
    
    public int DepartmentId { get; set; }
    
    public Department? Department { get; set; }

    public List<EmployeeTask>? EmployeeTasks { get; set; } = new();
}