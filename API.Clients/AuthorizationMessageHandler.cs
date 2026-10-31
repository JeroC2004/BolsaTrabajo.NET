// Guía de estudio: Intercepta las solicitudes HTTP del cliente compartido y agrega la credencial de sesión antes de enviarlas a la API.
using System.Net.Http.Headers;

namespace API.Clients
{
    
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public AuthorizationMessageHandler() : base(new HttpClientHandler())
        {
        }

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public AuthorizationMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        // Intercepta la solicitud HTTP para incorporar la autenticación antes de delegar su envío al siguiente manejador.
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var authService = AuthServiceProvider.Instance;

            await authService.CheckTokenExpirationAsync();

            string? token = await authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
