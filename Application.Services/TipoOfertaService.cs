// Guía de estudio: Servicio de aplicación de TipoOferta: coordina las operaciones de sus repositorios y prepara los DTO que intercambia con la API. Es el punto donde revisar la lógica del caso de uso.
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class TipoOfertaService : ITipoOfertaService
    {
        private readonly ITipoOfertaRepository tipoOfertaRepository;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public TipoOfertaService(ITipoOfertaRepository tipoOfertaRepository)
        {
            this.tipoOfertaRepository = tipoOfertaRepository;
        }

        // Obtiene la colección de registros coordinando los datos de esta capa.
        public async Task<IEnumerable<TipoOfertaDTO>> GetAllAsync()
        {
            var tipos = await tipoOfertaRepository.GetAllAsync();
            return tipos.Select(MapToDTO).ToList();
        }

        // Busca un registro por su identificador coordinando los datos de esta capa.
        public async Task<TipoOfertaDTO?> GetAsync(int id)
        {
            var tipo = await tipoOfertaRepository.GetAsync(id);
            return tipo == null ? null : MapToDTO(tipo);
        }

        // Agrega un registro coordinando los datos de esta capa.
        public async Task<TipoOfertaDTO> AddAsync(TipoOfertaDTO dto)
        {
            TipoOferta tipo = CrearTipo(0, dto);

            if (await tipoOfertaRepository.NameExistsAsync(tipo.Nombre))
                throw new ArgumentException("Ya existe un tipo de oferta con ese nombre.");

            await tipoOfertaRepository.AddAsync(tipo);
            return MapToDTO(tipo);
        }

        // Actualiza el registro o su estado coordinando los datos de esta capa.
        public async Task<bool> UpdateAsync(TipoOfertaDTO dto)
        {
            TipoOferta tipo = CrearTipo(dto.Id, dto);

            if (await tipoOfertaRepository.NameExistsAsync(tipo.Nombre, tipo.Id))
                throw new ArgumentException("Ya existe otro tipo de oferta con ese nombre.");

            return await tipoOfertaRepository.UpdateAsync(tipo);
        }

        // Elimina el registro indicado coordinando los datos de esta capa.
        public async Task<bool> DeleteAsync(int id)
        {
            if (await tipoOfertaRepository.HasOfertasAsync(id))
                throw new InvalidOperationException("No se puede eliminar el tipo de oferta porque tiene ofertas asociadas.");

            return await tipoOfertaRepository.DeleteAsync(id);
        }

        // Construye el tipo de oferta a partir de los datos validados del DTO.
        private static TipoOferta CrearTipo(int id, TipoOfertaDTO dto)
        {
            string nombre = (dto.Nombre ?? string.Empty).Trim();
            if (nombre.Length > 100)
                throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

            return new TipoOferta(id, nombre);
        }

        // Transforma la entidad o proyección en un DTO para devolver solamente los datos previstos por el contrato de la API.
        private static TipoOfertaDTO MapToDTO(TipoOferta tipo)
        {
            return new TipoOfertaDTO
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre
            };
        }
    }
}
