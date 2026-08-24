namespace Domain.Model
{
    public class Empresa
    {
        public int Id { get; private set; }
        public string RazonSocial { get; private set; }
        public string Descripcion { get; private set; }
        public string Rubro { get; private set; }

        public Empresa(int id, string razonSocial, string descripcion, string rubro)
        {
            SetId(id);
            SetRazonSocial(razonSocial);
            SetDescripcion(descripcion);
            SetRubro(rubro);
        }

        private Empresa() { } // Constructor privado requerido por EF Core

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        public void SetRazonSocial(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                throw new ArgumentException("La razón social no puede ser nula o vacía.", nameof(razonSocial));
            RazonSocial = razonSocial;
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción no puede ser nula o vacía.", nameof(descripcion));
            Descripcion = descripcion;
        }

        public void SetRubro(string rubro)
        {
            if (string.IsNullOrWhiteSpace(rubro))
                throw new ArgumentException("El rubro no puede ser nulo o vacío.", nameof(rubro));
            Rubro = rubro;
        }
    }
}
