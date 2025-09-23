using System.ComponentModel.DataAnnotations;

namespace WebClient.Application.DTOs.Security;

public class RegisterUserRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}