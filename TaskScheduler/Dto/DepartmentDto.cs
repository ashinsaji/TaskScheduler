using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.Dto;

public class DepartmentDto
{
    public int Id { get; set; }

    public required string Name { get; set; }
}

public class CreateDepartmentDto
{
    [Required(ErrorMessage = "Department name is required.")]
    public required string Name { get; set; }
}

public class UpdateDepartmentDto
{
    [Required(ErrorMessage = "Department name is required.")]
    public required string Name { get; set; }
}