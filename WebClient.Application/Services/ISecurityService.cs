using WebClient.Application.DTOs.Security;
using WebClient.Domain.Common;

namespace WebClient.Application.Services;

public interface ISecurityService
{
    Task<Result<UserDto>> RegisterAsync(RegisterUserRequestDto request);
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request);
}
