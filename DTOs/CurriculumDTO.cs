namespace DTOs
{
    public class CurriculumDTO
    {
        public int AlumnoId { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
        public int Tamanio { get; set; }
        public DateTime FechaCarga { get; set; }
    }

    public class CurriculumArchivoDTO
    {
        public string NombreArchivo { get; set; } = string.Empty;
        public byte[] Contenido { get; set; } = Array.Empty<byte>();
    }
}
