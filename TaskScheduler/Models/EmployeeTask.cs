using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.Models;

public class EmployeeTask
{
    public int TaskId { get; set; }
    public Task? Task { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    [Required]
    [StringLength(100)]
    public required string PriorityCode { get; set; }
}