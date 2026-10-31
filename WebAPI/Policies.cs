// Guía de estudio: Centraliza los nombres de políticas de autorización. Program.cs define qué roles satisface cada política y los endpoints la exigen.
namespace WebAPI
{
    public static class Policies
    {
        public const string Administrador = "Administrador";
        public const string Empresa = "Empresa";
        public const string AdministradorOEmpresa = "AdministradorOEmpresa";
        public const string AdministradorOAlumno = "AdministradorOAlumno";
    }
}
