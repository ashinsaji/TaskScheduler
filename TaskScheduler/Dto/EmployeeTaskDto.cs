using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.Dto;

public class EmployeeTaskDto
{
    public required int EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required int TaskId { get; set; }
    public required string TaskName { get; set; }
    public required string PriorityCode { get; set; } 
}

public class CreateEmployeeTaskDto
{
    [Required(ErrorMessage = "EmployeeId is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "TaskId is required.")]
    public int TaskId { get; set; }
    [Required(ErrorMessage = "PriorityCode is required.")]
    public required string PriorityCode { get; set; }
}

public class UpdateEmployeeTaskDto
{
    [Required(ErrorMessage = "PriorityCode is required.")]
    public required string PriorityCode { get; set; }
}   