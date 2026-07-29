using Data;
using DTOs;

namespace Application.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository empresaRepository;

        public EmpresaService(IEmpresaRepository empresaRepository)
        {
            this.empresaRepository = empresaRepository;
        }

        public async Task<IEnumerable<EmpresaDTO>> GetAllAsync()
        {
            var empresas = await empresaRepository.GetAllAsync();
            return empresas.Select(e => new EmpresaDTO
            {
                Id = e.Id,
                RazonSocial = e.RazonSocial,
                Descripcion = e.Descripcion,
                Rubro = e.Rubro
            }).ToList();
        }

        public async Task<EmpresaDTO?> GetAsync(int id)
        {
            var empresa = await empresaRepository.GetAsync(id);
            if (empresa == null)
                return null;

            return new EmpresaDTO
            {
                Id = empresa.Id,
                RazonSocial = empresa.RazonSocial,
                Descripcion = empresa.Descripcion,
                Rubro = empresa.Rubro
            };
        }
    }
}
