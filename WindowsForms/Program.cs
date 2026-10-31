// Guía de estudio: Punto de entrada del cliente de escritorio: prepara Windows Forms y la autenticación e inicia el flujo de pantallas de la aplicación.
using API.Auth.WindowsForms;
using API.Clients;
using System.Net.Http;

namespace WindowsForms
{
    internal static class Program
    {
        [STAThread]
        // Inicializa la interfaz de escritorio, registra la sesión y abre el formulario principal.
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
