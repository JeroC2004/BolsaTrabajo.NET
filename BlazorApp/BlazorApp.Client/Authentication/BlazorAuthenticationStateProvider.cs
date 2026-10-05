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

        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Name, username)
            },
            authenticationType: "Bearer");

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