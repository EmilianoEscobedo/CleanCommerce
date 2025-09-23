namespace WebClient.Application.DTOs.Security;

public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
}