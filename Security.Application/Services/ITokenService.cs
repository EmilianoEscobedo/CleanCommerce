using Security.Application.DTOs;
using Security.Domain.Entities;

namespace Security.Application.Services;

public interface ITokenService
{
    TokenDto GenerateToken(User user);
    bool ValidateToken(string token);
}
