using Microsoft.AspNetCore.Identity;

namespace CourseApi.Entities;

public class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }


    public virtual List<UserCourse> Courses { get; set; }
    public virtual List<UserTask> UserTasks { get; set; }
}
