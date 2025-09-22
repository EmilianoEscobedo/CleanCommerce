namespace Security.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime? UpdatedDate { get; private set; }

    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
        CreatedDate = DateTime.UtcNow;
    }
}