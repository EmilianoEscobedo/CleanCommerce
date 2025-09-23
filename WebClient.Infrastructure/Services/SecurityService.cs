using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WebClient.Application.DTOs.Security;
using WebClient.Application.Services;
using WebClient.Domain.Common;
using WebClient.Infrastructure.Settings;

namespace WebClient.Infrastructure.Services;

public class SecurityService : ISecurityService
{
    private readonly HttpClient _httpClient;
    private readonly ClientSettings _settings;

    public SecurityService(HttpClient httpClient, IOptions<ClientSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<Result<RegisterUserResponseDto>> RegisterAsync(RegisterUserRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_settings.SecurityBaseUrl}/register", request);
        if (!response.IsSuccessStatusCode)
            return Result<RegisterUserResponseDto>.Failure($"Failed to register user. Status: {response.StatusCode}");

        var user = await response.Content.ReadFromJsonAsync<RegisterUserResponseDto>();
        return user is null
            ? Result<RegisterUserResponseDto>.Failure("User deserialization failed.")
            : Result<RegisterUserResponseDto>.Success(user);
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_settings.SecurityBaseUrl}/login", request);
        
        if (!response.IsSuccessStatusCode)
            return Result<LoginResponseDto>.Failure($"Failed to login. Status: {response.StatusCode}");

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        return loginResponse is null
            ? Result<LoginResponseDto>.Failure("Login response deserialization failed.")
            : Result<LoginResponseDto>.Success(loginResponse);
    }
}