using API.Clients;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Client.Authentication;

public sealed class BlazorAuthenticationStateProvider
    : AuthenticationStateProvider
{
    private readonly TokenStorage tokenStorage;

    public BlazorAuthenticationStateProvider(TokenStorage tokenStorage)
    {
        this.tokenStorage = tokenStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = await tokenStorage.GetTokenAsync();
        string? username = await tokenStorage.GetUsernameAsync();

        if (string.IsNullOrWhiteSpace(token) ||
            string.IsNullOrWhiteSpace(username))
        {
            return Anonymous();
        }

        // Nombre, rol, alumnoId y empresaId se leen del propio JWT. El cliente solo los usa para
        // mostrar u ocultar opciones; la autorización real la valida la API.
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

    public async Task NotifyLoginAsync(string token, string username)
    {
        await tokenStorage.SaveAsync(token, username);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task NotifyLogoutAsync()
    {
        await tokenStorage.ClearAsync();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static AuthenticationState Anonymous()
    {
        return new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity()));
    }
}