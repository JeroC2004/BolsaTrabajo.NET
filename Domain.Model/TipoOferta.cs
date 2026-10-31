// Guía de estudio: Entidad de dominio TipoOferta: representa los datos del catálogo y ofrece métodos para construir o modificar sus valores.
namespace Domain.Model
{
    public class TipoOferta
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; }

        // Construye la entidad a partir de los datos recibidos y aplica sus reglas de inicialización.
        public TipoOferta(int id, string nombre)
        {
            SetId(id);
            SetNombre(nombre);
        }

        // Constructor sin parámetros utilizado por Entity Framework Core al materializar la entidad desde la base de datos.
        private TipoOferta() { }

        // Establece Id aplicando las comprobaciones definidas en este método.
        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        // Establece Nombre aplicando las comprobaciones definidas en este método.
        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nombre));
            Nombre = nombre;
        }
    }
}
