// Guía de estudio: Servicio de aplicación de Carrera: coordina las operaciones de sus repositorios y prepara los DTO que intercambia con la API. Es el punto donde revisar la lógica del caso de uso.
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class CarreraService : ICarreraService
    {
        private readonly ICarreraRepository carreraRepository;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public CarreraService(ICarreraRepository carreraRepository)
        {
            this.carreraRepository = carreraRepository;
        }

        // Obtiene la colección de registros coordinando los datos de esta capa.
        public async Task<IEnumerable<CarreraDTO>> GetAllAsync()
        {
            var carreras = await carreraRepository.GetAllAsync();
            return carreras.Select(MapToDTO).ToList();
        }

        // Busca un registro por su identificador coordinando los datos de esta capa.
        public async Task<CarreraDTO?> GetAsync(int id)
        {
            var carrera = await carreraRepository.GetAsync(id);
            return carrera == null ? null : MapToDTO(carrera);
        }

        // Agrega un registro coordinando los datos de esta capa.
        public async Task<CarreraDTO> AddAsync(CarreraDTO dto)
        {
            Carrera carrera = CrearCarrera(0, dto);

            if (await carreraRepository.NameExistsAsync(carrera.NomCarrera))
                throw new ArgumentException("Ya existe una carrera con ese nombre.");

            await carreraRepository.AddAsync(carrera);
            return MapToDTO(carrera);
        }

        // Actualiza el registro o su estado coordinando los datos de esta capa.
        public async Task<bool> UpdateAsync(CarreraDTO dto)
        {
            Carrera carrera = CrearCarrera(dto.Id, dto);

            if (await carreraRepository.NameExistsAsync(carrera.NomCarrera, carrera.Id))
                throw new ArgumentException("Ya existe otra carrera con ese nombre.");

            return await carreraRepository.UpdateAsync(carrera);
        }

        // Elimina el registro indicado coordinando los datos de esta capa.
        public async Task<bool> DeleteAsync(int id)
        {
            if (await carreraRepository.HasAlumnosAsync(id))
                throw new InvalidOperationException("No se puede eliminar la carrera porque tiene alumnos asociados.");

            return await carreraRepository.DeleteAsync(id);
        }

        // Construye la entidad Carrera con los campos normalizados y validados del DTO.
        private static Carrera CrearCarrera(int id, CarreraDTO dto)
        {
            return new Carrera(
                id,
                Limpiar(dto.NomCarrera, 150, "El nombre de la carrera"),
                Limpiar(dto.Departamento, 100, "El departamento"),
                dto.Duracion);
        }

        // Normaliza el texto y verifica las restricciones del campo antes de construir la entidad.
        private static string Limpiar(string? valor, int maximo, string campo)
        {
            string texto = (valor ?? string.Empty).Trim();
            if (texto.Length > maximo)
                throw new ArgumentException($"{campo} no puede superar los {maximo} caracteres.");
            return texto;
        }

        // Transforma la entidad o proyección en un DTO para devolver solamente los datos previstos por el contrato de la API.
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
