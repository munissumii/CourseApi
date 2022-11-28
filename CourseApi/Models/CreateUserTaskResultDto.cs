using CourseApi.Entities;

namespace CourseApi.Models;

public class CreateUserTaskResultDto
{
    public string? Description { get; set; }
    public EUserTask Status { get; set; }
}
