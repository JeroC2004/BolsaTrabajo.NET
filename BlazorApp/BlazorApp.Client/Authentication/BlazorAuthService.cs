// Guía de estudio: Implementa la sesión web: solicita el JWT, coordina su almacenamiento y avisa al proveedor de autenticación cuando cambia la sesión.
using API.Clients;
using DTOs;
using Microsoft.JSInterop;

namespace BlazorApp.Client.Authentication;

public sealed class BlazorAuthService : IAuthService
{
    private const string TokenKey = "bolsaTrabajoToken";
    private const string UsernameKey = "bolsaTrabajoUsername";
    private const string ExpirationKey = "bolsaTrabajoExpiration";

    private readonly AuthApiClient _authClient;
    private readonly IJSRuntime jsRuntime;

    public event Action<bool>? AuthenticationStateChanged;
    // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
    public BlazorAuthService(
        AuthApiClient authClient,
        IJSRuntime jsRuntime)
    {
        _authClient = authClient;
        this.jsRuntime = jsRuntime;
    }

    // Comprueba las credenciales mediante la implementación de autenticación de esta capa y devuelve el resultado del acceso.
    public async Task<bool> LoginAsync(string username, string password)
    {
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };

        LoginResponse? loginResponse = await _authClient.LoginAsync(request);

        if (loginResponse == null || string.IsNullOrWhiteSpace(loginResponse.Token))
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

    // Recupera el token de la sesión para autorizar las solicitudes HTTP.
    public async Task<string?> GetTokenAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            TokenKey);
    }

    // Obtiene el nombre de usuario guardado en la sesión.
    public async Task<string?> GetUsernameAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            UsernameKey);
    }

    // Indica si la sesión dispone de las credenciales que esta implementación considera vigentes.
    public async Task<bool> IsAuthenticatedAsync()
    {
        string? token = await GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return false;

        await CheckTokenExpirationAsync();

        token = await GetTokenAsync();

        return !string.IsNullOrWhiteSpace(token);
    }

    // Comprueba si el token sigue vigente y actualiza la sesión cuando corresponde.
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

    // Limpia la sesión de esta interfaz para que deje de utilizar las credenciales guardadas.
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