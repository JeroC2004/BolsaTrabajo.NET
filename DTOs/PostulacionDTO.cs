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

        public int OfertaId { get; set; }
        public string? OfertaTitulo { get; set; }
        public int EmpresaId { get; set; }
        public string? EmpresaNombre { get; set; }

        public DateTime FechaPostulacion { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
