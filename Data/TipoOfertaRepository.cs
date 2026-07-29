using Domain.Model;

namespace Data
{
    public class TipoOfertaRepository : ITipoOfertaRepository
    {
        private static readonly List<TipoOferta> tiposOferta = new List<TipoOferta>
        {
            new TipoOferta(1, "Pasantía"),
            new TipoOferta(2, "Primer empleo"),
            new TipoOferta(3, "Práctica profesional supervisada"),
            new TipoOferta(4, "Empleo full-time")
        };

        public Task<IEnumerable<TipoOferta>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<TipoOferta>>(tiposOferta.OrderBy(t => t.Nombre).ToList());
        }

        // Método interno sincrónico para uso desde OfertaRepository
        internal IEnumerable<TipoOferta> GetAllSync()
        {
            return tiposOferta.OrderBy(t => t.Nombre).ToList();
        }
    }
}
