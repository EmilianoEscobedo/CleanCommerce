namespace Security.Application.DTOs;

public class TokenDto
{
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
}