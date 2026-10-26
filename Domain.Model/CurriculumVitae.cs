namespace Domain.Model
{
    /// <summary>
    /// CV en PDF de un alumno. Cada alumno tiene como máximo uno; al cargar otro se reemplaza.
    /// </summary>
    public class CurriculumVitae
    {
        public const int TamanioMaximoBytes = 5 * 1024 * 1024; // 5 MB
        public const int LongitudMaximaNombre = 200;

        private static readonly byte[] FirmaPdf = "%PDF-"u8.ToArray();

        public int Id { get; private set; }
        public int AlumnoId { get; private set; }
        public string NombreArchivo { get; private set; } = string.Empty;
        public byte[] Contenido { get; private set; } = Array.Empty<byte>();
        public int Tamanio { get; private set; }
        public DateTime FechaCarga { get; private set; }

        public CurriculumVitae(int id, int alumnoId, string nombreArchivo, byte[] contenido, DateTime fechaCarga)
        {
            SetId(id);
            SetAlumnoId(alumnoId);
            Reemplazar(nombreArchivo, contenido, fechaCarga);
        }

        private CurriculumVitae() { }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        public void SetAlumnoId(int alumnoId)
        {
            if (alumnoId <= 0)
                throw new ArgumentException("El AlumnoId debe ser mayor que 0.", nameof(alumnoId));
            AlumnoId = alumnoId;
        }

        // Valida todo antes de asignar, para no dejar el objeto a medias si algo falla.
        public void Reemplazar(string nombreArchivo, byte[] contenido, DateTime fechaCarga)
        {
            string nombre = ValidarNombre(nombreArchivo);
            ValidarContenido(contenido);

            if (fechaCarga == default)
                throw new ArgumentException("La fecha de carga no es válida.", nameof(fechaCarga));

            NombreArchivo = nombre;
            Contenido = contenido;
            Tamanio = contenido.Length;
            FechaCarga = fechaCarga;
        }

        private static string ValidarNombre(string nombreArchivo)
        {
            if (string.IsNullOrWhiteSpace(nombreArchivo))
                throw new ArgumentException("El nombre del archivo no puede ser nulo o vacío.", nameof(nombreArchivo));

            // Se descarta cualquier ruta que venga del cliente: solo interesa el nombre.
            string nombre = Path.GetFileName(nombreArchivo.Replace('\\', '/')).Trim();

            if (!nombre.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("El currículum debe ser un archivo PDF (.pdf).", nameof(nombreArchivo));

            if (nombre.Length > LongitudMaximaNombre)
                throw new ArgumentException($"El nombre del archivo no puede superar los {LongitudMaximaNombre} caracteres.", nameof(nombreArchivo));

            return nombre;
        }

        private static void ValidarContenido(byte[] contenido)
        {
            if (contenido == null || contenido.Length == 0)
                throw new ArgumentException("El archivo está vacío.", nameof(contenido));

            if (contenido.Length > TamanioMaximoBytes)
                throw new ArgumentException($"El archivo supera el tamaño máximo de {TamanioMaximoBytes / (1024 * 1024)} MB.", nameof(contenido));

            // No alcanza con mirar la extensión: se verifica que realmente sea un PDF.
            if (!contenido.AsSpan().StartsWith(FirmaPdf))
                throw new ArgumentException("El archivo no es un PDF válido.", nameof(contenido));
        }
    }

    /// <summary>Datos del CV sin el contenido binario, para listar o consultar sin cargar el archivo.</summary>
    public record CurriculumResumen(int AlumnoId, string NombreArchivo, int Tamanio, DateTime FechaCarga);
}
