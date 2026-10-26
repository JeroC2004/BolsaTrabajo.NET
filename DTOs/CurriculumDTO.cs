namespace DTOs
{
    /// <summary>Metadatos del CV de un alumno (sin el archivo).</summary>
    public class CurriculumDTO
    {
        public int AlumnoId { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
        public int Tamanio { get; set; }
        public DateTime FechaCarga { get; set; }
    }

    /// <summary>CV completo, usado para la descarga.</summary>
    public class CurriculumArchivoDTO
    {
        public string NombreArchivo { get; set; } = string.Empty;
        public byte[] Contenido { get; set; } = Array.Empty<byte>();
    }
}
