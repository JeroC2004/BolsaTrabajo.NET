using Domain.Model;

namespace Data
{
    public class OfertaRepository : IOfertaRepository
    {
        private static readonly List<Oferta> ofertas = new List<Oferta>();
        private static int nextId = 1;

        public Task AddAsync(Oferta oferta)
        {
            // Simular auto-increment de ID
            oferta.SetId(nextId);
            nextId++;

            // Asignar navigation properties
            var empresaRepo = new EmpresaRepository();
            var empresa = empresaRepo.GetAllSync().FirstOrDefault(e => e.Id == oferta.EmpresaId);
            if (empresa != null)
                oferta.SetEmpresa(empresa);

            var tipoOfertaRepo = new TipoOfertaRepository();
            var tipoOferta = tipoOfertaRepo.GetAllSync().FirstOrDefault(t => t.Id == oferta.TipoOfertaId);
            if (tipoOferta != null)
                oferta.SetTipoOferta(tipoOferta);

            ofertas.Add(oferta);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var oferta = ofertas.FirstOrDefault(o => o.Id == id);
            if (oferta != null)
            {
                ofertas.Remove(oferta);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Oferta?> GetAsync(int id)
        {
            return Task.FromResult(ofertas.FirstOrDefault(o => o.Id == id));
        }

        public Task<IEnumerable<Oferta>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Oferta>>(ofertas.ToList());
        }

        public Task<bool> UpdateAsync(Oferta oferta)
        {
            var existing = ofertas.FirstOrDefault(o => o.Id == oferta.Id);
            if (existing != null)
            {
                existing.SetTitulo(oferta.Titulo);
                existing.SetTipoVinculo(oferta.TipoVinculo);
                existing.SetFechaDesde(oferta.FechaDesde);
                existing.SetFechaHasta(oferta.FechaHasta);
                existing.SetDetalle(oferta.Detalle);
                existing.SetRequisitos(oferta.Requisitos);
                existing.SetEstado(oferta.Estado);
                existing.SetEmpresaId(oferta.EmpresaId);
                existing.SetTipoOfertaId(oferta.TipoOfertaId);

                var empresaRepo = new EmpresaRepository();
                var empresa = empresaRepo.GetAllSync().FirstOrDefault(e => e.Id == oferta.EmpresaId);
                if (empresa != null)
                    existing.SetEmpresa(empresa);

                var tipoOfertaRepo = new TipoOfertaRepository();
                var tipoOferta = tipoOfertaRepo.GetAllSync().FirstOrDefault(t => t.Id == oferta.TipoOfertaId);
                if (tipoOferta != null)
                    existing.SetTipoOferta(tipoOferta);

                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<Oferta>> GetByCriteriaAsync(OfertaCriteria criteria)
        {
            string searchTerm = criteria.Texto.ToLower();

            IEnumerable<Oferta> result = ofertas.Where(o =>
                o.Titulo.ToLower().Contains(searchTerm) ||
                o.Detalle.ToLower().Contains(searchTerm) ||
                o.Estado.ToLower().Contains(searchTerm)
            ).OrderByDescending(o => o.FechaDesde).ToList();

            return Task.FromResult(result);
        }
    }
}
