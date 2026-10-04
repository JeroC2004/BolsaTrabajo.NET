using System.Net.Http.Json;
using DTOs;

namespace BlazorApp.Client.Services;

public sealed class BlazorAuthService
{
    private readonly HttpClient _httpClient;
    private readonly TokenStorage _tokenStorage;

    public BlazorAuthService(
        HttpClient httpClient,
        TokenStorage tokenStorage)
    {
        _httpClient = httpClient;
        _tokenStorage = tokenStorage;
    }

    public event Action<bool>? AuthenticationStateChanged;

    public async Task<bool> LoginAsync(string username, string password)
    {
        LoginRequest request = new()
        {
            Username = username,
            Password = password
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
            "auth/login",
            request);

        if (!response.IsSuccessStatusCode)
            return false;

        LoginResponse? loginResponse =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (loginResponse is null || string.IsNullOrWhiteSpace(loginResponse.Token))
            return false;

        await _tokenStorage.SetAsync(
            loginResponse.Token,
            loginResponse.Username);

        AuthenticationStateChanged?.Invoke(true);

        return true;
    }

    public async Task LogoutAsync()
    {
        await _tokenStorage.ClearAsync();
        AuthenticationStateChanged?.Invoke(false);
    }

    public Task<string?> GetTokenAsync()
    {
        return _tokenStorage.GetTokenAsync();
    }

    public Task<string?> GetUsernameAsync()
    {
        return _tokenStorage.GetUsernameAsync();
    }
}