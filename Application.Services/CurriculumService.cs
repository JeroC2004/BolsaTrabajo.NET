using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class CurriculumService : ICurriculumService
    {
        private readonly ICurriculumRepository curriculumRepository;
        private readonly IAlumnoRepository alumnoRepository;

        public CurriculumService(ICurriculumRepository curriculumRepository, IAlumnoRepository alumnoRepository)
        {
            this.curriculumRepository = curriculumRepository;
            this.alumnoRepository = alumnoRepository;
        }

        public async Task<CurriculumDTO?> GetInfoAsync(int alumnoId)
        {
            CurriculumResumen? resumen = await curriculumRepository.GetResumenAsync(alumnoId);
            return resumen == null ? null : MapToDTO(resumen);
        }

        public async Task<CurriculumArchivoDTO?> GetArchivoAsync(int alumnoId)
        {
            CurriculumVitae? cv = await curriculumRepository.GetConContenidoAsync(alumnoId);
            if (cv == null)
                return null;

            return new CurriculumArchivoDTO { NombreArchivo = cv.NombreArchivo, Contenido = cv.Contenido };
        }

        public async Task<CurriculumDTO> GuardarAsync(int alumnoId, string nombreArchivo, byte[] contenido)
        {
            if (await alumnoRepository.GetAsync(alumnoId) == null)
                throw new ArgumentException("El alumno indicado no existe.");

            // La entidad valida extensión, tamaño y que el contenido sea realmente un PDF.
            var cv = new CurriculumVitae(0, alumnoId, nombreArchivo, contenido, DateTime.Now);

            await curriculumRepository.GuardarAsync(cv);

            return MapToDTO(new CurriculumResumen(cv.AlumnoId, cv.NombreArchivo, cv.Tamanio, cv.FechaCarga));
        }

        public async Task<bool> DeleteAsync(int alumnoId)
        {
            return await curriculumRepository.DeleteAsync(alumnoId);
        }

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
