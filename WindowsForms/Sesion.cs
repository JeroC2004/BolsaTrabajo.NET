// Guía de estudio: Expone rol e identificadores de la sesión de escritorio a partir de los claims del token, para adaptar pantallas y acciones al usuario.
using API.Clients;
using DTOs;
using System.Security.Claims;

namespace WindowsForms
{
    
    internal static class Sesion
    {
        public static string Rol { get; private set; } = string.Empty;
        public static int? AlumnoId { get; private set; }
        public static int? EmpresaId { get; private set; }

        public static bool EsAdministrador => Rol == Roles.Administrador;
        public static bool EsEmpresa => Rol == Roles.Empresa;
        public static bool EsAlumno => Rol == Roles.Alumno;

        // Carga los datos necesarios y actualiza los controles de la pantalla.
        public static async Task CargarAsync()
        {
            string? token = await AuthServiceProvider.Instance.GetTokenAsync();
            var claims = JwtClaimsReader.ReadClaims(token);

            Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;
            AlumnoId = LeerEntero(claims, AuthClaims.AlumnoId);
            EmpresaId = LeerEntero(claims, AuthClaims.EmpresaId);
        }

        // Extrae un claim y devuelve su valor entero, o null si no existe o no se puede convertir.
        private static int? LeerEntero(IEnumerable<Claim> claims, string tipo)
        {
            return int.TryParse(claims.FirstOrDefault(c => c.Type == tipo)?.Value, out int valor) ? valor : null;
        }
    }
}
