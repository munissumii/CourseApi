using System.ComponentModel.DataAnnotations;

namespace CourseApi.Models;

public class UpdateCourseDto
{
    [Required]
    public string Name { get; set; }    
}
