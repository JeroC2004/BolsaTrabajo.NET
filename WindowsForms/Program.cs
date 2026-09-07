using API.Auth.WindowsForms;
using API.Clients;

namespace WindowsForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Registra la implementación de IAuthService que va a usar toda la
            // capa API.Clients para agregar el token JWT a cada request.
            AuthServiceProvider.Register(new WindowsFormsAuthService());

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
