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
            var tipoVinculo = ParseTipoVinculo(dto.TipoVinculo);
            var estado = ParseEstado(dto.Estado);

            Oferta oferta = new Oferta(0, dto.Titulo, tipoVinculo, dto.FechaDesde, dto.FechaHasta,
                                        dto.Detalle, estado, dto.EmpresaId, dto.TipoOfertaId);
            oferta.SetRequisitos(MapRequisitos(dto.Requisitos, conservarIds: false));

            await ofertaRepository.AddAsync(oferta);

            return await GetAsync(oferta.Id) ?? throw new InvalidOperationException("No se pudo recuperar la oferta recién creada.");
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

            var tipoVinculo = ParseTipoVinculo(dto.TipoVinculo);
            var estado = ParseEstado(dto.Estado);

            Oferta oferta = new Oferta(dto.Id, dto.Titulo, tipoVinculo, dto.FechaDesde, dto.FechaHasta,
                                        dto.Detalle, estado, dto.EmpresaId, dto.TipoOfertaId);
            oferta.SetRequisitos(MapRequisitos(dto.Requisitos, conservarIds: true));

            return await ofertaRepository.UpdateAsync(oferta);
        }

        public async Task<IEnumerable<OfertaDTO>> GetByCriteriaAsync(OfertaCriteriaDTO criteriaDTO)
        {
            var criteria = new OfertaCriteria(criteriaDTO.Texto);
            var ofertas = await ofertaRepository.GetByCriteriaAsync(criteria);
            return ofertas.Select(MapToDTO).ToList();
        }

        private static TipoVinculo ParseTipoVinculo(string value)
        {
            if (!Enum.TryParse<TipoVinculo>(value, ignoreCase: true, out var result))
                throw new ArgumentException($"El tipo de vínculo '{value}' no es válido. Valores permitidos: {string.Join(", ", Enum.GetNames(typeof(TipoVinculo)))}.");
            return result;
        }

        private static EstadoOferta ParseEstado(string value)
        {
            if (!Enum.TryParse<EstadoOferta>(value, ignoreCase: true, out var result))
                throw new ArgumentException($"El estado '{value}' no es válido. Valores permitidos: {string.Join(", ", Enum.GetNames(typeof(EstadoOferta)))}.");
            return result;
        }

        private static List<RequisitoOferta> MapRequisitos(IEnumerable<RequisitoOfertaDTO>? requisitos, bool conservarIds)
        {
            if (requisitos == null)
                return new List<RequisitoOferta>();

            return requisitos.Select(r =>
            {
                if (string.IsNullOrWhiteSpace(r.Descripcion))
                    throw new ArgumentException("La descripción de un requisito no puede ser nula o vacía.");

                return new RequisitoOferta
                {
                    // En el alta todas las líneas son nuevas (Id = 0). En la modificación el Id
                    // permite que el repositorio distinga líneas existentes de líneas nuevas.
                    Id = conservarIds ? r.Id : 0,
                    Descripcion = r.Descripcion.Trim(),
                    EsExcluyente = r.EsExcluyente
                };
            }).ToList();
        }

        private static OfertaDTO MapToDTO(Oferta oferta)
        {
            return new OfertaDTO
            {
                Id = oferta.Id,
                Titulo = oferta.Titulo,
                TipoVinculo = oferta.TipoVinculo.ToString(),
                Estado = oferta.Estado.ToString(),
                FechaDesde = oferta.FechaDesde,
                FechaHasta = oferta.FechaHasta,
                Detalle = oferta.Detalle,
                Requisitos = oferta.Requisitos
                    .Select(r => new RequisitoOfertaDTO
                    {
                        Id = r.Id,
                        Descripcion = r.Descripcion,
                        EsExcluyente = r.EsExcluyente
                    })
                    .ToList(),
                EmpresaId = oferta.EmpresaId,
                EmpresaNombre = oferta.Empresa?.RazonSocial,
                TipoOfertaId = oferta.TipoOfertaId,
                TipoOfertaNombre = oferta.TipoOferta?.Nombre
            };
        }
    }
}
