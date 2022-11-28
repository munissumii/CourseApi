using System.ComponentModel.DataAnnotations;

namespace CourseApi.Models;

public class SignUpDto
{ 

    [Required]  
    public string? UserName { get; set; }
    [Required]
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [Required]
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}
