using Data;
using Domain.Model;
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
            return carreras.Select(MapToDTO).ToList();
        }

        public async Task<CarreraDTO?> GetAsync(int id)
        {
            var carrera = await carreraRepository.GetAsync(id);
            return carrera == null ? null : MapToDTO(carrera);
        }

        public async Task<CarreraDTO> AddAsync(CarreraDTO dto)
        {
            Carrera carrera = CrearCarrera(0, dto);

            if (await carreraRepository.NameExistsAsync(carrera.NomCarrera))
                throw new ArgumentException("Ya existe una carrera con ese nombre.");

            await carreraRepository.AddAsync(carrera);
            return MapToDTO(carrera);
        }

        public async Task<bool> UpdateAsync(CarreraDTO dto)
        {
            Carrera carrera = CrearCarrera(dto.Id, dto);

            if (await carreraRepository.NameExistsAsync(carrera.NomCarrera, carrera.Id))
                throw new ArgumentException("Ya existe otra carrera con ese nombre.");

            return await carreraRepository.UpdateAsync(carrera);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (await carreraRepository.HasAlumnosAsync(id))
                throw new InvalidOperationException("No se puede eliminar la carrera porque tiene alumnos asociados.");

            return await carreraRepository.DeleteAsync(id);
        }

        private static Carrera CrearCarrera(int id, CarreraDTO dto)
        {
            return new Carrera(
                id,
                Limpiar(dto.NomCarrera, 150, "El nombre de la carrera"),
                Limpiar(dto.Departamento, 100, "El departamento"),
                dto.Duracion);
        }

        private static string Limpiar(string? valor, int maximo, string campo)
        {
            string texto = (valor ?? string.Empty).Trim();
            if (texto.Length > maximo)
                throw new ArgumentException($"{campo} no puede superar los {maximo} caracteres.");
            return texto;
        }

        private static CarreraDTO MapToDTO(Carrera carrera)
        {
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
