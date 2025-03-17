using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.Dto;

public class TaskDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Procedure { get; set; }
    public int SopId { get; set; }
}

public class CreateTaskDto
{
    [Required(ErrorMessage = "Task name is required.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Procedure is required.")]
    public required string Procedure { get; set; }

    [Required(ErrorMessage = "SOP ID is required.")]
    public int SopId { get; set; }
}

public class UpdateTaskDto
{
    [Required(ErrorMessage = "Task name is required.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Procedure is required.")]
    public required string Procedure { get; set; }

    [Required(ErrorMessage = "SOP ID is required.")]
    public int SopId { get; set; }
}