using System.Net;
using System.Net.Http.Headers;

namespace BlazorApp.Client.Authentication;

public sealed class BearerTokenHandler : DelegatingHandler
{
    private readonly TokenStorage tokenStorage;
    private readonly BlazorAuthenticationStateProvider authenticationProvider;

    public BearerTokenHandler(
        TokenStorage tokenStorage,
        BlazorAuthenticationStateProvider authenticationProvider)
    {
        this.tokenStorage = tokenStorage;
        this.authenticationProvider = authenticationProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string? token = await tokenStorage.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        HttpResponseMessage response =
            await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await authenticationProvider.NotifyLogoutAsync();
        }

        return response;
    }
}