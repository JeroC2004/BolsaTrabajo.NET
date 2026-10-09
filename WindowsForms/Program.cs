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

            using (LoginForm loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    bool salir = false;
                    while (!salir)
                    {
                        using (Home homeForm = new Home())
                        {
                            Application.Run(homeForm);
                        }

                        var authService = AuthServiceProvider.Instance;
                        if (authService.IsAuthenticatedAsync().Result)
                        {
                            salir = true;
                        }
                        else
                        {
                            using (LoginForm nuevoLogin = new LoginForm())
                            {
                                if (nuevoLogin.ShowDialog() != DialogResult.OK)
                                    salir = true;
                            }
                        }
                    }
                }
            }
        }
    }
}