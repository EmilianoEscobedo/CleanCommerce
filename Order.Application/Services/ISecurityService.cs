namespace Order.Application.Services;

public interface ISecurityService
{
    Task<bool> ValidateTokenAsync(string token);
}
