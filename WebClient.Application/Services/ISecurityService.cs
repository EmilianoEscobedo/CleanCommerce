using WebClient.Application.DTOs.Security;
using WebClient.Domain.Common;

namespace WebClient.Application.Services;

public interface ISecurityService
{
    Task<Result<RegisterUserResponseDto>> RegisterAsync(RegisterUserRequestDto request);
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request);
}
