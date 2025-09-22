using Security.Application.DTOs;
using Security.Domain.Commons;

namespace Security.Application.Services;

public interface IAuthService
{
    Task<Result<UserDto>> RegisterAsync(RegisterUserDto registerDto);
    Task<Result<TokenDto>> LoginAsync(LoginDto loginDto);
    Result<bool> ValidateToken(string token);
}