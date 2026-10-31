// Guía de estudio: Contrato de datos PostulacionDTO: agrupa los campos intercambiados entre API y clientes. Las propiedades describen el contenido de la solicitud o respuesta; no realizan acceso a base de datos.
namespace DTOs
{
    public class PostulacionDTO
    {
        public int Id { get; set; }

        public int AlumnoId { get; set; }
        public string? AlumnoNombre { get; set; }
        public string? AlumnoLegajo { get; set; }
        public string? AlumnoEmail { get; set; }
        public string? AlumnoCarrera { get; set; }
        public float AlumnoPromedio { get; set; }
        public bool AlumnoTieneCv { get; set; }

        public int OfertaId { get; set; }
        public string? OfertaTitulo { get; set; }
        public int EmpresaId { get; set; }
        public string? EmpresaNombre { get; set; }

        public DateTime FechaPostulacion { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
