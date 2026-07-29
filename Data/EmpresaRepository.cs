using Domain.Model;

namespace Data
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private static readonly List<Empresa> empresas = new List<Empresa>
        {
            new Empresa(1, "TechCorp S.A.", "Empresa de desarrollo de software", "Tecnología"),
            new Empresa(2, "Banco Litoral", "Entidad financiera regional", "Finanzas"),
            new Empresa(3, "Agro Insumos S.R.L.", "Distribuidora de insumos agropecuarios", "Agroindustria")
        };

        public Task<IEnumerable<Empresa>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Empresa>>(empresas.OrderBy(e => e.RazonSocial).ToList());
        }

        public Task<Empresa?> GetAsync(int id)
        {
            return Task.FromResult(empresas.FirstOrDefault(e => e.Id == id));
        }

        // Método interno sincrónico para uso desde OfertaRepository
        internal IEnumerable<Empresa> GetAllSync()
        {
            return empresas.OrderBy(e => e.RazonSocial).ToList();
        }
    }
}
