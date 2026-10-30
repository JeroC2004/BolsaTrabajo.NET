using API.Auth.WindowsForms;
using API.Clients;
using System.Net.Http;

namespace WindowsForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var httpClient = new HttpClient { BaseAddress = new Uri(ApiConfiguration.BaseUrl) };

            var authClient = new AuthApiClient(httpClient);

            AuthServiceProvider.Register(new WindowsFormsAuthService(authClient));

            Application.Run(new Home());
        }
    }
}
