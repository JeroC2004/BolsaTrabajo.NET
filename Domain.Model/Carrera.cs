namespace Domain.Model
{
    public class Carrera
    {
        public int Id { get; private set; }
        public string NomCarrera { get; private set; }
        public string Departamento { get; private set; }
        public int Duracion { get; private set; }

        public Carrera(int id, string nomCarrera, string departamento, int duracion)
        {
            SetId(id);
            SetNomCarrera(nomCarrera);
            SetDepartamento(departamento);
            SetDuracion(duracion);
        }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        public void SetNomCarrera(string nomCarrera)
        {
            if (string.IsNullOrWhiteSpace(nomCarrera))
                throw new ArgumentException("El nombre de la carrera no puede ser nulo o vacío.", nameof(nomCarrera));
            NomCarrera = nomCarrera;
        }

        public void SetDepartamento(string departamento)
        {
            if (string.IsNullOrWhiteSpace(departamento))
                throw new ArgumentException("El departamento no puede ser nulo o vacío.", nameof(departamento));
            Departamento = departamento;
        }

        public void SetDuracion(int duracion)
        {
            if (duracion <= 0)
                throw new ArgumentException("La duración debe ser mayor que 0.", nameof(duracion));
            Duracion = duracion;
        }
    }
}
