using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Product.Application.Services;
using Product.Infrastructure.Settings;

namespace Product.Infrastructure.Services;

public class SecurityService : ISecurityService
{
    private readonly HttpClient _httpClient;
    private readonly ClientSettings _settings;

    public SecurityService(HttpClient httpClient, IOptions<ClientSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_settings.SecurityBaseUrl}/validate", token);

        if (!response.IsSuccessStatusCode)
            return false;

        var isValid = await response.Content.ReadFromJsonAsync<bool>();
        return isValid;
    }
}