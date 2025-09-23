namespace WebClient.Application.DTOs.Security;

public class RegisterUserResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}