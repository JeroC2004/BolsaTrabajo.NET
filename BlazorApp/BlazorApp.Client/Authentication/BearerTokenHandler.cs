// Guía de estudio: Manejador HTTP de Blazor que adjunta el JWT como Authorization: Bearer a las solicitudes de los clientes protegidos.
using System.Net;
using System.Net.Http.Headers;

namespace BlazorApp.Client.Authentication;

public sealed class BearerTokenHandler : DelegatingHandler
{
    private readonly TokenStorage tokenStorage;
    private readonly BlazorAuthenticationStateProvider authenticationProvider;

    // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
    public BearerTokenHandler(
        TokenStorage tokenStorage,
        BlazorAuthenticationStateProvider authenticationProvider)
    {
        this.tokenStorage = tokenStorage;
        this.authenticationProvider = authenticationProvider;
    }

    // Intercepta la solicitud HTTP para incorporar la autenticación antes de delegar su envío al siguiente manejador.
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