using Domain.Model;

namespace Data
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private static readonly List<Alumno> alumnos = new List<Alumno>();
        private static int nextId = 1;

        public Task AddAsync(Alumno alumno)
        {
            // Simular auto-increment de ID
            alumno.SetId(nextId);
            nextId++;

            // Asignar navigation property de Carrera
            var carreraRepo = new CarreraRepository();
            var carrera = carreraRepo.GetAllSync().FirstOrDefault(c => c.Id == alumno.CarreraId);
            if (carrera != null)
                alumno.SetCarrera(carrera);

            alumnos.Add(alumno);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var alumno = alumnos.FirstOrDefault(a => a.Id == id);
            if (alumno != null)
            {
                alumnos.Remove(alumno);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Alumno?> GetAsync(int id)
        {
            return Task.FromResult(alumnos.FirstOrDefault(a => a.Id == id));
        }

        public Task<IEnumerable<Alumno>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Alumno>>(alumnos.ToList());
        }

        public Task<bool> UpdateAsync(Alumno alumno)
        {
            var existing = alumnos.FirstOrDefault(a => a.Id == alumno.Id);
            if (existing != null)
            {
                existing.SetNomAlumno(alumno.NomAlumno);
                existing.SetApeAlumno(alumno.ApeAlumno);
                existing.SetEmail(alumno.Email);
                existing.SetLegajo(alumno.Legajo);
                existing.SetDni(alumno.Dni);
                existing.SetPlan(alumno.Plan);
                existing.SetAnioCurso(alumno.AnioCurso);
                existing.SetCantMatAp(alumno.CantMatAp);
                existing.SetPromedio(alumno.Promedio);
                existing.SetCarreraId(alumno.CarreraId);

                var carreraRepo = new CarreraRepository();
                var carrera = carreraRepo.GetAllSync().FirstOrDefault(c => c.Id == alumno.CarreraId);
                if (carrera != null)
                    existing.SetCarrera(carrera);

                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = alumnos.Where(a => a.Email.ToLower() == email.ToLower());
            if (excludeId.HasValue)
                query = query.Where(a => a.Id != excludeId.Value);
            return Task.FromResult(query.Any());
        }

        public Task<bool> LegajoExistsAsync(string legajo, int? excludeId = null)
        {
            var query = alumnos.Where(a => a.Legajo.ToLower() == legajo.ToLower());
            if (excludeId.HasValue)
                query = query.Where(a => a.Id != excludeId.Value);
            return Task.FromResult(query.Any());
        }

        public Task<IEnumerable<Alumno>> GetByCriteriaAsync(AlumnoCriteria criteria)
        {
            string searchTerm = criteria.Texto.ToLower();

            IEnumerable<Alumno> result = alumnos.Where(a =>
                a.NomAlumno.ToLower().Contains(searchTerm) ||
                a.ApeAlumno.ToLower().Contains(searchTerm) ||
                a.Legajo.ToLower().Contains(searchTerm) ||
                a.Email.ToLower().Contains(searchTerm)
            ).OrderBy(a => a.ApeAlumno).ThenBy(a => a.NomAlumno).ToList();

            return Task.FromResult(result);
        }
    }
}
