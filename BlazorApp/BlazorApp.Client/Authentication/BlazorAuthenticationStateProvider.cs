// Guía de estudio: Construye el estado de autenticación de Blazor a partir del JWT guardado y notifica cambios para actualizar rutas y vistas autorizadas.
using API.Clients;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Client.Authentication;

public sealed class BlazorAuthenticationStateProvider
    : AuthenticationStateProvider
{
    private readonly TokenStorage tokenStorage;

    // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
    public BlazorAuthenticationStateProvider(TokenStorage tokenStorage)
    {
        this.tokenStorage = tokenStorage;
    }

    // Construye el usuario autenticado a partir del token válido en el cliente o devuelve una identidad anónima.
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = await tokenStorage.GetTokenAsync();
        string? username = await tokenStorage.GetUsernameAsync();

        if (string.IsNullOrWhiteSpace(token) ||
            string.IsNullOrWhiteSpace(username))
        {
            return Anonymous();
        }

        var claims = JwtClaimsReader.ReadClaims(token).ToList();

        if (JwtClaimsReader.IsExpired(claims))
        {
            return Anonymous();
        }

        if (!claims.Any(c => c.Type == ClaimTypes.Name))
        {
            claims.Add(new Claim(ClaimTypes.Name, username));
        }

        var identity = new ClaimsIdentity(
            claims,
            authenticationType: "Bearer",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    // Guarda la nueva sesión y notifica a Blazor para actualizar las vistas autorizadas.
    public async Task NotifyLoginAsync(string token, string username)
    {
        await tokenStorage.SaveAsync(token, username);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // Borra la sesión guardada y notifica a Blazor que el usuario pasó a ser anónimo.
    public async Task NotifyLogoutAsync()
    {
        await tokenStorage.ClearAsync();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // Construye un estado de autenticación con identidad anónima, sin una sesión iniciada.
    private static AuthenticationState Anonymous()
    {
        return new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity()));
    }
}