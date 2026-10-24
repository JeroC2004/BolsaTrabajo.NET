using DTOs;
using System.Security.Claims;

namespace WebAPI
{
    public static class UserClaimsExtensions
    {
        public static bool EsAdministrador(this ClaimsPrincipal user) => user.IsInRole(Roles.Administrador);

        public static bool EsEmpresa(this ClaimsPrincipal user) => user.IsInRole(Roles.Empresa);

        public static bool EsAlumno(this ClaimsPrincipal user) => user.IsInRole(Roles.Alumno);

        public static int? GetAlumnoId(this ClaimsPrincipal user) => LeerEntero(user, AuthClaims.AlumnoId);

        public static int? GetEmpresaId(this ClaimsPrincipal user) => LeerEntero(user, AuthClaims.EmpresaId);

        private static int? LeerEntero(ClaimsPrincipal user, string tipo)
        {
            return int.TryParse(user.FindFirst(tipo)?.Value, out int valor) ? valor : null;
        }
    }
}
