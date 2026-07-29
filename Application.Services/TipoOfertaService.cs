using Data;
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
            return tipos.Select(t => new TipoOfertaDTO
            {
                Id = t.Id,
                Nombre = t.Nombre
            }).ToList();
        }
    }
}
