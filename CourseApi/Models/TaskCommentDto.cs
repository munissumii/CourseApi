namespace CourseApi.Models;

public class TaskCommentDto
{
    public Guid Id { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedDate { get; set; }

    public virtual List<TaskCommentDto> Children { get; set; }
    public virtual UserDto? User { get; set; }
}
