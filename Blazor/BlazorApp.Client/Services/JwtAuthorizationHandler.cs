using System.Net;
using System.Net.Http.Headers;

namespace BlazorApp.Client.Services;

public sealed class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly BlazorAuthService _authService;

    public JwtAuthorizationHandler(BlazorAuthService authService)
    {
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string? token = await _authService.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        HttpResponseMessage response =
            await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            await _authService.LogoutAsync();

        return response;
    }
}