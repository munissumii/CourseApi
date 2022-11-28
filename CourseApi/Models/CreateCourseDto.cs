using System.ComponentModel.DataAnnotations;

namespace CourseApi.Models;

public class CreateCourseDto
{
    [Required]
    public string Name { get; set; }


}
