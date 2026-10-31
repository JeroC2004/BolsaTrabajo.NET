// Guía de estudio: Centraliza los nombres de roles y los tipos de claims compartidos para que API e interfaces usen los mismos identificadores.
namespace DTOs
{

    public static class Roles
    {
        public const string Administrador = "Administrador";
        public const string Empresa = "Empresa";
        public const string Alumno = "Alumno";

        public const string AdministradorOEmpresa = Administrador + "," + Empresa;
        public const string AdministradorOAlumno = Administrador + "," + Alumno;
    }

    public static class AuthClaims
    {
        public const string AlumnoId = "alumnoId";
        public const string EmpresaId = "empresaId";
    }
}
