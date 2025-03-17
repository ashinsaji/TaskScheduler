using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.Models;

public class Department
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }
    
    public List<Employee>? Employees { get; set; }
    public List<Sop>? Sops { get; set; }
}