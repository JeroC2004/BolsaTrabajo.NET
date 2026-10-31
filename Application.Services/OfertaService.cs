// Guía de estudio: Servicio de aplicación de Oferta: coordina las operaciones de sus repositorios y prepara los DTO que intercambia con la API. Es el punto donde revisar la lógica del caso de uso.
using Domain.Model;
using Data;
using DTOs;

namespace Application.Services
{
    public class OfertaService : IOfertaService
    {
        private readonly IOfertaRepository ofertaRepository;
        private readonly IEmpresaRepository empresaRepository;
        private readonly ITipoOfertaRepository tipoOfertaRepository;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public OfertaService(
            IOfertaRepository ofertaRepository,
            IEmpresaRepository empresaRepository,
            ITipoOfertaRepository tipoOfertaRepository)
        {
            this.ofertaRepository = ofertaRepository;
            this.empresaRepository = empresaRepository;
            this.tipoOfertaRepository = tipoOfertaRepository;
        }

        // Agrega un registro coordinando los datos de esta capa.
        public async Task<OfertaDTO> AddAsync(OfertaDTO dto)
        {
            var tipoVinculo = ParseTipoVinculo(dto.TipoVinculo);
            var estado = ParseEstado(dto.Estado);

            Oferta oferta = new Oferta(0, dto.Titulo, tipoVinculo, dto.FechaDesde, dto.FechaHasta,
                                        dto.Detalle, estado, dto.EmpresaId, dto.TipoOfertaId);
            oferta.SetRequisitos(MapRequisitos(dto.Requisitos, conservarIds: false));

            await ValidarReferenciasAsync(dto.EmpresaId, dto.TipoOfertaId);

            await ofertaRepository.AddAsync(oferta);

            return await GetAsync(oferta.Id) ?? throw new InvalidOperationException("No se pudo recuperar la oferta recién creada.");
        }

        // Elimina el registro indicado coordinando los datos de esta capa.
        public async Task<bool> DeleteAsync(int id)
        {
            return await ofertaRepository.DeleteAsync(id);
        }

        // Busca un registro por su identificador coordinando los datos de esta capa.
        public async Task<OfertaDTO?> GetAsync(int id)
        {
            Oferta? oferta = await ofertaRepository.GetAsync(id);

            if (oferta == null)
                return null;

            return MapToDTO(oferta);
        }

        // Obtiene la colección de registros coordinando los datos de esta capa.
        public async Task<IEnumerable<OfertaDTO>> GetAllAsync()
        {
            var ofertas = await ofertaRepository.GetAllAsync();
            return ofertas.Select(MapToDTO).ToList();
        }

        // Actualiza el registro o su estado coordinando los datos de esta capa.
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

            await ValidarReferenciasAsync(dto.EmpresaId, dto.TipoOfertaId);

            return await ofertaRepository.UpdateAsync(oferta);
        }

        // Consulta registros con los criterios de búsqueda recibidos coordinando los datos de esta capa.
        public async Task<IEnumerable<OfertaDTO>> GetByCriteriaAsync(OfertaCriteriaDTO criteriaDTO)
        {
            var criteria = new OfertaCriteria(criteriaDTO.Texto, criteriaDTO.TipoOfertaId,
                string.IsNullOrWhiteSpace(criteriaDTO.Estado) ? null : ParseEstado(criteriaDTO.Estado),
                string.IsNullOrWhiteSpace(criteriaDTO.TipoVinculo) ? null : ParseTipoVinculo(criteriaDTO.TipoVinculo),
                criteriaDTO.Desde, criteriaDTO.Hasta);
            var ofertas = await ofertaRepository.GetByCriteriaAsync(criteria);
            return ofertas.Select(MapToDTO).ToList();
        }

        // Convierte el texto recibido a TipoVinculo y rechaza los valores que no puede interpretar.
        private static TipoVinculo ParseTipoVinculo(string value)
        {
            if (!Enum.TryParse<TipoVinculo>(value, ignoreCase: true, out var result))
                throw new ArgumentException($"El tipo de vínculo '{value}' no es válido. Valores permitidos: {string.Join(", ", Enum.GetNames(typeof(TipoVinculo)))}.");
            return result;
        }

        // Convierte el texto recibido a Estado y rechaza los valores que no puede interpretar.
        private static EstadoOferta ParseEstado(string value)
        {
            if (!Enum.TryParse<EstadoOferta>(value, ignoreCase: true, out var result))
                throw new ArgumentException($"El estado '{value}' no es válido. Valores permitidos: {string.Join(", ", Enum.GetNames(typeof(EstadoOferta)))}.");
            return result;
        }

        // Comprueba ReferenciasAsync antes de continuar con la operación.
        private async Task ValidarReferenciasAsync(int empresaId, int tipoOfertaId)
        {
            if (await empresaRepository.GetAsync(empresaId) == null)
                throw new ArgumentException($"La empresa con Id {empresaId} no existe.");

            var tipoOfertaExiste = (await tipoOfertaRepository.GetAllAsync()).Any(t => t.Id == tipoOfertaId);
            if (!tipoOfertaExiste)
                throw new ArgumentException($"El tipo de oferta con Id {tipoOfertaId} no existe.");
        }

        // Convierte requisitos recibidos a entidades; valida descripciones y decide si conserva los identificadores al editar.
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

                    Id = conservarIds ? r.Id : 0,
                    Descripcion = r.Descripcion.Trim(),
                    EsExcluyente = r.EsExcluyente
                };
            }).ToList();
        }

        // Transforma la entidad o proyección en un DTO para devolver solamente los datos previstos por el contrato de la API.
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
