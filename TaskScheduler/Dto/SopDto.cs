using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.Dto;

public class SopDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int DepartmentId { get; set; }
}

public class CreateSopDto
{
    [Required(ErrorMessage = "SOP name is required.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Department ID is required.")]
    public int DepartmentId { get; set; }
}

public class UpdateSopDto
{
    [Required(ErrorMessage = "SOP name is required.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Department ID is required.")]
    public int DepartmentId { get; set; }
}