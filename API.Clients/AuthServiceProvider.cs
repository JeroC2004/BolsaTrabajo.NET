// Guía de estudio: Mantiene la implementación de autenticación que usa el cliente de escritorio; permite registrarla, consultarla y quitarla.
namespace API.Clients
{
    public static class AuthServiceProvider
    {
        private static IAuthService? _instance;
        private static readonly object _lock = new object();

        public static IAuthService Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("AuthService no fue registrado. Llamar a AuthServiceProvider.Register() primero.");
                return _instance;
            }
        }

        // Registra el servicio de autenticación que usarán las llamadas del cliente de escritorio.
        public static void Register(IAuthService authService)
        {
            lock (_lock)
            {
                _instance = authService;
            }
        }

        // Quita la referencia al servicio de autenticación registrado.
        public static void Clear()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }
    }
}
