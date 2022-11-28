using System.ComponentModel.DataAnnotations;

namespace CourseApi.Models;

public class SignInDto
{
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? Password { get; set; }
}
