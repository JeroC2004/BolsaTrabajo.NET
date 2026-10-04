using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Client.Services;

public sealed class JwtAuthenticationStateProvider
    : AuthenticationStateProvider
{
    private readonly TokenStorage _tokenStorage;
    private readonly BlazorAuthService _authService;

    public JwtAuthenticationStateProvider(
        TokenStorage tokenStorage,
        BlazorAuthService authService)
    {
        _tokenStorage = tokenStorage;
        _authService = authService;

        _authService.AuthenticationStateChanged +=
            OnAuthenticationStateChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return Anonymous();

        ClaimsIdentity identity = new(
            ParseClaimsFromJwt(token),
            "jwt");

        return new AuthenticationState(
            new ClaimsPrincipal(identity));
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(
            GetAuthenticationStateAsync());
    }

    private void OnAuthenticationStateChanged(bool authenticated)
    {
        NotifyAuthenticationStateChanged();
    }

    private static AuthenticationState Anonymous()
    {
        return new AuthenticationState(
            new ClaimsPrincipal(
                new ClaimsIdentity()));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        string payload = token.Split('.')[1];

        byte[] jsonBytes = ParseBase64WithoutPadding(payload);

        Dictionary<string, object>? claims =
            JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (claims is null)
            return [];

        return claims.Select(claim =>
            new Claim(claim.Key, claim.Value.ToString() ?? string.Empty));
    }

    private static byte[] ParseBase64WithoutPadding(string payload)
    {
        payload = payload.Replace('-', '+').Replace('_', '/');

        int padding = 4 - payload.Length % 4;

        if (padding != 4)
            payload += new string('=', padding);

        return Convert.FromBase64String(payload);
    }
}