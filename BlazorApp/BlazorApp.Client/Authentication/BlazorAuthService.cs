using System.Net.Http.Json;
using API.Clients;
using DTOs;
using Microsoft.JSInterop;

namespace BlazorApp.Client.Authentication;

public sealed class BlazorAuthService : IAuthService
{
    private const string TokenKey = "bolsaTrabajoToken";
    private const string UsernameKey = "bolsaTrabajoUsername";
    private const string ExpirationKey = "bolsaTrabajoExpiration";

    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    public event Action<bool>? AuthenticationStateChanged;

    public BlazorAuthService(
        HttpClient httpClient,
        IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };

        using HttpResponseMessage response =
            await httpClient.PostAsJsonAsync("auth/login", request);

        if (!response.IsSuccessStatusCode)
            return false;

        LoginResponse? loginResponse =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (loginResponse == null ||
            string.IsNullOrWhiteSpace(loginResponse.Token))
        {
            return false;
        }

        await jsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            TokenKey,
            loginResponse.Token);

        await jsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            UsernameKey,
            loginResponse.Username);

        await jsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            ExpirationKey,
            loginResponse.ExpiresAt.ToString("O"));

        AuthenticationStateChanged?.Invoke(true);

        return true;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            TokenKey);
    }

    public async Task<string?> GetUsernameAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            UsernameKey);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        string? token = await GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return false;

        await CheckTokenExpirationAsync();

        token = await GetTokenAsync();

        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task CheckTokenExpirationAsync()
    {
        string? expirationValue =
            await jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                ExpirationKey);

        if (!DateTime.TryParse(
                expirationValue,
                out DateTime expiration))
        {
            return;
        }

        if (expiration <= DateTime.UtcNow)
            await LogoutAsync();
    }

    public async Task LogoutAsync()
    {
        await jsRuntime.InvokeVoidAsync(
            "localStorage.removeItem",
            TokenKey);

        await jsRuntime.InvokeVoidAsync(
            "localStorage.removeItem",
            UsernameKey);

        await jsRuntime.InvokeVoidAsync(
            "localStorage.removeItem",
            ExpirationKey);

        AuthenticationStateChanged?.Invoke(false);
    }
}