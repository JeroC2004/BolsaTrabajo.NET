// Guía de estudio: Entidad de dominio Carrera: representa los datos del catálogo y ofrece métodos para construir o modificar sus valores.
namespace Domain.Model
{
    public class Carrera
    {
        public int Id { get; private set; }
        public string NomCarrera { get; private set; }
        public string Departamento { get; private set; }
        public int Duracion { get; private set; }

        // Construye la entidad a partir de los datos recibidos y aplica sus reglas de inicialización.
        public Carrera(int id, string nomCarrera, string departamento, int duracion)
        {
            SetId(id);
            SetNomCarrera(nomCarrera);
            SetDepartamento(departamento);
            SetDuracion(duracion);
        }

        // Constructor sin parámetros utilizado por Entity Framework Core al materializar la entidad desde la base de datos.
        private Carrera() { }

        // Establece Id aplicando las comprobaciones definidas en este método.
        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        // Establece NomCarrera aplicando las comprobaciones definidas en este método.
        public void SetNomCarrera(string nomCarrera)
        {
            if (string.IsNullOrWhiteSpace(nomCarrera))
                throw new ArgumentException("El nombre de la carrera no puede ser nulo o vacío.", nameof(nomCarrera));
            NomCarrera = nomCarrera;
        }

        // Establece Departamento aplicando las comprobaciones definidas en este método.
        public void SetDepartamento(string departamento)
        {
            if (string.IsNullOrWhiteSpace(departamento))
                throw new ArgumentException("El departamento no puede ser nulo o vacío.", nameof(departamento));
            Departamento = departamento;
        }

        // Establece Duracion aplicando las comprobaciones definidas en este método.
        public void SetDuracion(int duracion)
        {
            if (duracion <= 0)
                throw new ArgumentException("La duración debe ser mayor que 0.", nameof(duracion));
            Duracion = duracion;
        }
    }
}
