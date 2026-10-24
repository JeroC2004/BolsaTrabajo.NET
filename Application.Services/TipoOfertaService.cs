using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class TipoOfertaService : ITipoOfertaService
    {
        private readonly ITipoOfertaRepository tipoOfertaRepository;

        public TipoOfertaService(ITipoOfertaRepository tipoOfertaRepository)
        {
            this.tipoOfertaRepository = tipoOfertaRepository;
        }

        public async Task<IEnumerable<TipoOfertaDTO>> GetAllAsync()
        {
            var tipos = await tipoOfertaRepository.GetAllAsync();
            return tipos.Select(MapToDTO).ToList();
        }

        public async Task<TipoOfertaDTO?> GetAsync(int id)
        {
            var tipo = await tipoOfertaRepository.GetAsync(id);
            return tipo == null ? null : MapToDTO(tipo);
        }

        public async Task<TipoOfertaDTO> AddAsync(TipoOfertaDTO dto)
        {
            TipoOferta tipo = CrearTipo(0, dto);

            if (await tipoOfertaRepository.NameExistsAsync(tipo.Nombre))
                throw new ArgumentException("Ya existe un tipo de oferta con ese nombre.");

            await tipoOfertaRepository.AddAsync(tipo);
            return MapToDTO(tipo);
        }

        public async Task<bool> UpdateAsync(TipoOfertaDTO dto)
        {
            TipoOferta tipo = CrearTipo(dto.Id, dto);

            if (await tipoOfertaRepository.NameExistsAsync(tipo.Nombre, tipo.Id))
                throw new ArgumentException("Ya existe otro tipo de oferta con ese nombre.");

            return await tipoOfertaRepository.UpdateAsync(tipo);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (await tipoOfertaRepository.HasOfertasAsync(id))
                throw new InvalidOperationException("No se puede eliminar el tipo de oferta porque tiene ofertas asociadas.");

            return await tipoOfertaRepository.DeleteAsync(id);
        }

        private static TipoOferta CrearTipo(int id, TipoOfertaDTO dto)
        {
            string nombre = (dto.Nombre ?? string.Empty).Trim();
            if (nombre.Length > 100)
                throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

            return new TipoOferta(id, nombre);
        }

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
