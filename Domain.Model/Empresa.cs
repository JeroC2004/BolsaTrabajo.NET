// Guía de estudio: Entidad de dominio Empresa: representa los datos del catálogo y ofrece métodos para construir o modificar sus valores.
namespace Domain.Model
{
    public class Empresa
    {
        public int Id { get; private set; }
        public string RazonSocial { get; private set; }
        public string Descripcion { get; private set; }
        public string Rubro { get; private set; }

        // Construye la entidad a partir de los datos recibidos y aplica sus reglas de inicialización.
        public Empresa(int id, string razonSocial, string descripcion, string rubro)
        {
            SetId(id);
            SetRazonSocial(razonSocial);
            SetDescripcion(descripcion);
            SetRubro(rubro);
        }

        // Constructor sin parámetros utilizado por Entity Framework Core al materializar la entidad desde la base de datos.
        private Empresa() { }

        // Establece Id aplicando las comprobaciones definidas en este método.
        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        // Establece RazonSocial aplicando las comprobaciones definidas en este método.
        public void SetRazonSocial(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                throw new ArgumentException("La razón social no puede ser nula o vacía.", nameof(razonSocial));
            RazonSocial = razonSocial;
        }

        // Establece Descripcion aplicando las comprobaciones definidas en este método.
        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción no puede ser nula o vacía.", nameof(descripcion));
            Descripcion = descripcion;
        }

        // Establece Rubro aplicando las comprobaciones definidas en este método.
        public void SetRubro(string rubro)
        {
            if (string.IsNullOrWhiteSpace(rubro))
                throw new ArgumentException("El rubro no puede ser nulo o vacío.", nameof(rubro));
            Rubro = rubro;
        }
    }
}
