// Guía de estudio: Vincula un alumno con una oferta e incluye fecha y estado de la solicitud. La entidad controla sus valores; el servicio comprueba disponibilidad y duplicados.
namespace Domain.Model
{
    public class Postulacion
    {
        public int Id { get; private set; }
        public int AlumnoId { get; private set; }
        public Alumno? Alumno { get; private set; }
        public int OfertaId { get; private set; }
        public Oferta? Oferta { get; private set; }
        public DateTime FechaPostulacion { get; private set; }
        public EstadoPostulacion Estado { get; private set; }

        // Construye la entidad a partir de los datos recibidos y aplica sus reglas de inicialización.
        public Postulacion(int id, int alumnoId, int ofertaId, DateTime fechaPostulacion,
                           EstadoPostulacion estado = EstadoPostulacion.Pendiente)
        {
            SetId(id);
            SetAlumnoId(alumnoId);
            SetOfertaId(ofertaId);
            SetFechaPostulacion(fechaPostulacion);
            SetEstado(estado);
        }

        // Constructor sin parámetros utilizado por Entity Framework Core al materializar la entidad desde la base de datos.
        private Postulacion() { }

        // Establece Id aplicando las comprobaciones definidas en este método.
        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        // Establece AlumnoId aplicando las comprobaciones definidas en este método.
        public void SetAlumnoId(int alumnoId)
        {
            if (alumnoId <= 0)
                throw new ArgumentException("El AlumnoId debe ser mayor que 0.", nameof(alumnoId));
            AlumnoId = alumnoId;
        }

        // Establece OfertaId aplicando las comprobaciones definidas en este método.
        public void SetOfertaId(int ofertaId)
        {
            if (ofertaId <= 0)
                throw new ArgumentException("El OfertaId debe ser mayor que 0.", nameof(ofertaId));
            OfertaId = ofertaId;
        }

        // Establece FechaPostulacion aplicando las comprobaciones definidas en este método.
        public void SetFechaPostulacion(DateTime fechaPostulacion)
        {
            if (fechaPostulacion == default)
                throw new ArgumentException("La fecha de postulación no es válida.", nameof(fechaPostulacion));
            FechaPostulacion = fechaPostulacion;
        }

        // Establece Estado aplicando las comprobaciones definidas en este método.
        public void SetEstado(EstadoPostulacion estado)
        {
            if (!Enum.IsDefined(typeof(EstadoPostulacion), estado))
                throw new ArgumentException("El estado de la postulación no es válido.", nameof(estado));
            Estado = estado;
        }
    }
}
