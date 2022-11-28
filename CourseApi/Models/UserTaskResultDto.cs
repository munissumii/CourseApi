using CourseApi.Entities;

namespace CourseApi.Models;

public class UserTaskResultDto : TaskDto
{
    public UserTaskResultDto? UserResult { get; set; }
}

public class UserTaskResult
{
    public string? Description { get; set; }
    public EUserTask? Status { get; set; }
}

public class UsersTaskResult : UserTaskResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

}
