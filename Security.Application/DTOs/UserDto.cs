namespace Security.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}