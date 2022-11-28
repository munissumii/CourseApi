using CourseApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = CourseApi.Entities.Task;

namespace CourseApi.Context
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions option) : base(option)
        {
        }

        public  DbSet<Course> Courses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<TaskComment> Comments { get; set; }
    }
}
