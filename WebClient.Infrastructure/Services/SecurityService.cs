using System.Net.Http.Json;
using System.Text.Json;
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

    public async Task<Result<UserDto>> RegisterAsync(RegisterUserRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_settings.SecurityBaseUrl}/register", request);
        if (!response.IsSuccessStatusCode)
            return Result<UserDto>.Failure($"Failed to register user. Status: {response.StatusCode}");

        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        return user is null
            ? Result<UserDto>.Failure("User deserialization failed.")
            : Result<UserDto>.Success(user);
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        var json = JsonSerializer.Serialize(request);
        Console.WriteLine(json);

        var response = await _httpClient.PostAsJsonAsync($"{_settings.SecurityBaseUrl}/login", request);
        
        if (!response.IsSuccessStatusCode)
            return Result<LoginResponseDto>.Failure($"Failed to login. Status: {response.StatusCode}");

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        return loginResponse is null
            ? Result<LoginResponseDto>.Failure("Login response deserialization failed.")
            : Result<LoginResponseDto>.Success(loginResponse);
    }
}