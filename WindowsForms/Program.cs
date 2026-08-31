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
                    // Login exitoso: se muestra la pantalla principal.
                    // Cuando el usuario cierra sesión desde el menú, Home se cierra
                    // y volvemos a mostrar el login (loop hasta que cierre la app).
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
                            // Se cerró Home sin cerrar sesión (ej: Alt+F4) -> salir de la app
                            salir = true;
                        }
                        else
                        {
                            // Se cerró sesión -> volver a mostrar el login
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
