using System.Net.Http.Headers;

namespace API.Clients
{
    
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        public AuthorizationMessageHandler() : base(new HttpClientHandler())
        {
        }

        public AuthorizationMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

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
