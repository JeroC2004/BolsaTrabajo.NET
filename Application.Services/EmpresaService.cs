// Guía de estudio: Servicio de aplicación de Empresa: coordina las operaciones de sus repositorios y prepara los DTO que intercambia con la API. Es el punto donde revisar la lógica del caso de uso.
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository empresaRepository;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public EmpresaService(IEmpresaRepository empresaRepository)
        {
            this.empresaRepository = empresaRepository;
        }

        // Obtiene la colección de registros coordinando los datos de esta capa.
        public async Task<IEnumerable<EmpresaDTO>> GetAllAsync()
        {
            var empresas = await empresaRepository.GetAllAsync();
            return empresas.Select(MapToDTO).ToList();
        }

        // Busca un registro por su identificador coordinando los datos de esta capa.
        public async Task<EmpresaDTO?> GetAsync(int id)
        {
            var empresa = await empresaRepository.GetAsync(id);
            return empresa == null ? null : MapToDTO(empresa);
        }

        // Agrega un registro coordinando los datos de esta capa.
        public async Task<EmpresaDTO> AddAsync(EmpresaDTO dto)
        {
            Empresa empresa = CrearEmpresa(0, dto);

            if (await empresaRepository.NameExistsAsync(empresa.RazonSocial))
                throw new ArgumentException("Ya existe una empresa con esa razón social.");

            await empresaRepository.AddAsync(empresa);
            return MapToDTO(empresa);
        }

        // Actualiza el registro o su estado coordinando los datos de esta capa.
        public async Task<bool> UpdateAsync(EmpresaDTO dto)
        {
            Empresa empresa = CrearEmpresa(dto.Id, dto);

            if (await empresaRepository.NameExistsAsync(empresa.RazonSocial, empresa.Id))
                throw new ArgumentException("Ya existe otra empresa con esa razón social.");

            return await empresaRepository.UpdateAsync(empresa);
        }

        // Elimina el registro indicado coordinando los datos de esta capa.
        public async Task<bool> DeleteAsync(int id)
        {
            if (await empresaRepository.HasOfertasAsync(id))
                throw new InvalidOperationException("No se puede eliminar la empresa porque tiene ofertas asociadas.");

            return await empresaRepository.DeleteAsync(id);
        }

        // Construye la entidad Empresa con los campos normalizados y validados del DTO.
        private static Empresa CrearEmpresa(int id, EmpresaDTO dto)
        {
            return new Empresa(
                id,
                Limpiar(dto.RazonSocial, 150, "La razón social"),
                Limpiar(dto.Descripcion, 500, "La descripción"),
                Limpiar(dto.Rubro, 100, "El rubro"));
        }

        // Normaliza el texto y verifica las restricciones del campo antes de construir la entidad.
        private static string Limpiar(string? valor, int maximo, string campo)
        {
            string texto = (valor ?? string.Empty).Trim();
            if (texto.Length > maximo)
                throw new ArgumentException($"{campo} no puede superar los {maximo} caracteres.");
            return texto;
        }

        // Transforma la entidad o proyección en un DTO para devolver solamente los datos previstos por el contrato de la API.
        private static EmpresaDTO MapToDTO(Empresa empresa)
        {
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
