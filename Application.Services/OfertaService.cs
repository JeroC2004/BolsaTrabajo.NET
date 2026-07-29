using Domain.Model;
using Data;
using DTOs;

namespace Application.Services
{
    public class OfertaService : IOfertaService
    {
        private readonly IOfertaRepository ofertaRepository;

        public OfertaService(IOfertaRepository ofertaRepository)
        {
            this.ofertaRepository = ofertaRepository;
        }

        public async Task<OfertaDTO> AddAsync(OfertaDTO dto)
        {
            Oferta oferta = new Oferta(0, dto.Titulo, dto.TipoVinculo, dto.FechaDesde, dto.FechaHasta,
                                        dto.Detalle, dto.Requisitos, dto.Estado, dto.EmpresaId, dto.TipoOfertaId);

            await ofertaRepository.AddAsync(oferta);

            return MapToDTO(oferta);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ofertaRepository.DeleteAsync(id);
        }

        public async Task<OfertaDTO?> GetAsync(int id)
        {
            Oferta? oferta = await ofertaRepository.GetAsync(id);

            if (oferta == null)
                return null;

            return MapToDTO(oferta);
        }

        public async Task<IEnumerable<OfertaDTO>> GetAllAsync()
        {
            var ofertas = await ofertaRepository.GetAllAsync();
            return ofertas.Select(MapToDTO).ToList();
        }

        public async Task<bool> UpdateAsync(OfertaDTO dto)
        {
            var existing = await ofertaRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            Oferta oferta = new Oferta(dto.Id, dto.Titulo, dto.TipoVinculo, dto.FechaDesde, dto.FechaHasta,
                                        dto.Detalle, dto.Requisitos, dto.Estado, dto.EmpresaId, dto.TipoOfertaId);

            return await ofertaRepository.UpdateAsync(oferta);
        }

        public async Task<IEnumerable<OfertaDTO>> GetByCriteriaAsync(OfertaCriteriaDTO criteriaDTO)
        {
            var criteria = new OfertaCriteria(criteriaDTO.Texto);
            var ofertas = await ofertaRepository.GetByCriteriaAsync(criteria);
            return ofertas.Select(MapToDTO).ToList();
        }

        private static OfertaDTO MapToDTO(Oferta oferta)
        {
            return new OfertaDTO
            {
                Id = oferta.Id,
                Titulo = oferta.Titulo,
                TipoVinculo = oferta.TipoVinculo,
                FechaDesde = oferta.FechaDesde,
                FechaHasta = oferta.FechaHasta,
                Detalle = oferta.Detalle,
                Requisitos = oferta.Requisitos,
                Estado = oferta.Estado,
                EmpresaId = oferta.EmpresaId,
                EmpresaNombre = oferta.Empresa?.RazonSocial,
                TipoOfertaId = oferta.TipoOfertaId,
                TipoOfertaNombre = oferta.TipoOferta?.Nombre
            };
        }
    }
}
