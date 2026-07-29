using Data;
using DTOs;

namespace Application.Services
{
    public class CarreraService : ICarreraService
    {
        private readonly ICarreraRepository carreraRepository;

        public CarreraService(ICarreraRepository carreraRepository)
        {
            this.carreraRepository = carreraRepository;
        }

        public async Task<IEnumerable<CarreraDTO>> GetAllAsync()
        {
            var carreras = await carreraRepository.GetAllAsync();
            return carreras.Select(c => new CarreraDTO
            {
                Id = c.Id,
                NomCarrera = c.NomCarrera,
                Departamento = c.Departamento,
                Duracion = c.Duracion
            }).ToList();
        }

        public async Task<CarreraDTO?> GetAsync(int id)
        {
            var carrera = await carreraRepository.GetAsync(id);
            if (carrera == null)
                return null;

            return new CarreraDTO
            {
                Id = carrera.Id,
                NomCarrera = carrera.NomCarrera,
                Departamento = carrera.Departamento,
                Duracion = carrera.Duracion
            };
        }
    }
}
