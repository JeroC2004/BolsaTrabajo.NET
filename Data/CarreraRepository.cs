using Domain.Model;

namespace Data
{
    public class CarreraRepository : ICarreraRepository
    {
        private static readonly List<Carrera> carreras = new List<Carrera>
        {
            new Carrera(1, "Ingeniería en Sistemas de Información", "Ingeniería", 5),
            new Carrera(2, "Licenciatura en Administración", "Ciencias Económicas", 4),
            new Carrera(3, "Contador Público", "Ciencias Económicas", 5),
            new Carrera(4, "Ingeniería Industrial", "Ingeniería", 5)
        };

        public Task<IEnumerable<Carrera>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Carrera>>(carreras.OrderBy(c => c.NomCarrera).ToList());
        }

        public Task<Carrera?> GetAsync(int id)
        {
            return Task.FromResult(carreras.FirstOrDefault(c => c.Id == id));
        }

        // Método interno sincrónico para uso desde AlumnoRepository
        internal IEnumerable<Carrera> GetAllSync()
        {
            return carreras.OrderBy(c => c.NomCarrera).ToList();
        }
    }
}
