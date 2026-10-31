// Guía de estudio: Servicio de aplicación de Curriculum: coordina las operaciones de sus repositorios y prepara los DTO que intercambia con la API. Es el punto donde revisar la lógica del caso de uso.
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class CurriculumService : ICurriculumService
    {
        private readonly ICurriculumRepository curriculumRepository;
        private readonly IAlumnoRepository alumnoRepository;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public CurriculumService(ICurriculumRepository curriculumRepository, IAlumnoRepository alumnoRepository)
        {
            this.curriculumRepository = curriculumRepository;
            this.alumnoRepository = alumnoRepository;
        }

        // Consulta los metadatos del currículum del alumno sin descargar el PDF.
        public async Task<CurriculumDTO?> GetInfoAsync(int alumnoId)
        {
            CurriculumResumen? resumen = await curriculumRepository.GetResumenAsync(alumnoId);
            return resumen == null ? null : MapToDTO(resumen);
        }

        // Recupera el nombre y el contenido binario del currículum para su descarga.
        public async Task<CurriculumArchivoDTO?> GetArchivoAsync(int alumnoId)
        {
            CurriculumVitae? cv = await curriculumRepository.GetConContenidoAsync(alumnoId);
            if (cv == null)
                return null;

            return new CurriculumArchivoDTO { NombreArchivo = cv.NombreArchivo, Contenido = cv.Contenido };
        }

        // Reúne los datos editados y ejecuta el guardado, mostrando el resultado o los errores de la operación.
        public async Task<CurriculumDTO> GuardarAsync(int alumnoId, string nombreArchivo, byte[] contenido)
        {
            if (await alumnoRepository.GetAsync(alumnoId) == null)
                throw new ArgumentException("El alumno indicado no existe.");

            var cv = new CurriculumVitae(0, alumnoId, nombreArchivo, contenido, DateTime.Now);

            await curriculumRepository.GuardarAsync(cv);

            return MapToDTO(new CurriculumResumen(cv.AlumnoId, cv.NombreArchivo, cv.Tamanio, cv.FechaCarga));
        }

        // Elimina el registro indicado coordinando los datos de esta capa.
        public async Task<bool> DeleteAsync(int alumnoId)
        {
            return await curriculumRepository.DeleteAsync(alumnoId);
        }

        // Transforma la entidad o proyección en un DTO para devolver solamente los datos previstos por el contrato de la API.
        private static CurriculumDTO MapToDTO(CurriculumResumen r)
        {
            return new CurriculumDTO
            {
                AlumnoId = r.AlumnoId,
                NombreArchivo = r.NombreArchivo,
                Tamanio = r.Tamanio,
                FechaCarga = r.FechaCarga
            };
        }
    }
}
