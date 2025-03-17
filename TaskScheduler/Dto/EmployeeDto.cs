using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.Dto;

public class EmployeeDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int DepartmentId { get; set; }
}

public class CreateEmployeeDto
{
    [Required(ErrorMessage = "Employee name is required.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Department ID is required.")]
    public int DepartmentId { get; set; }
}

public class UpdateEmployeeDto
{
    [Required(ErrorMessage = "Employee name is required.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Department ID is required.")]
    public int DepartmentId { get; set; }
}