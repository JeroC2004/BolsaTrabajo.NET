// Guía de estudio: Extiende ClaimsPrincipal para consultar el rol y recuperar los identificadores de alumno o empresa incluidos en el JWT validado por la API.
using DTOs;
using System.Security.Claims;

namespace WebAPI
{
    public static class UserClaimsExtensions
    {
        // Indica si el usuario pertenece al rol Administrador.
        public static bool EsAdministrador(this ClaimsPrincipal user) => user.IsInRole(Roles.Administrador);

        // Indica si el usuario pertenece al rol Empresa.
        public static bool EsEmpresa(this ClaimsPrincipal user) => user.IsInRole(Roles.Empresa);

        // Indica si el usuario pertenece al rol Alumno.
        public static bool EsAlumno(this ClaimsPrincipal user) => user.IsInRole(Roles.Alumno);

        // Lee el identificador del alumno asociado al usuario desde sus claims.
        public static int? GetAlumnoId(this ClaimsPrincipal user) => LeerEntero(user, AuthClaims.AlumnoId);

        // Lee el identificador de la empresa asociada al usuario desde sus claims.
        public static int? GetEmpresaId(this ClaimsPrincipal user) => LeerEntero(user, AuthClaims.EmpresaId);

        // Extrae un claim y devuelve su valor entero, o null si no existe o no se puede convertir.
        private static int? LeerEntero(ClaimsPrincipal user, string tipo)
        {
            return int.TryParse(user.FindFirst(tipo)?.Value, out int valor) ? valor : null;
        }
    }
}
