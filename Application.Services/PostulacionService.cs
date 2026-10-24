using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class PostulacionService : IPostulacionService
    {
        private readonly IPostulacionRepository postulacionRepository;
        private readonly IAlumnoRepository alumnoRepository;
        private readonly IOfertaRepository ofertaRepository;

        public PostulacionService(
            IPostulacionRepository postulacionRepository,
            IAlumnoRepository alumnoRepository,
            IOfertaRepository ofertaRepository)
        {
            this.postulacionRepository = postulacionRepository;
            this.alumnoRepository = alumnoRepository;
            this.ofertaRepository = ofertaRepository;
        }

        public async Task<IEnumerable<PostulacionDTO>> GetAllAsync(int? alumnoId = null, int? empresaId = null, int? ofertaId = null)
        {
            var postulaciones = await postulacionRepository.GetAllAsync(alumnoId, empresaId, ofertaId);
            return postulaciones.Select(MapToDTO).ToList();
        }

        public async Task<PostulacionDTO?> GetAsync(int id)
        {
            var postulacion = await postulacionRepository.GetAsync(id);
            return postulacion == null ? null : MapToDTO(postulacion);
        }

        public async Task<PostulacionDTO> AddAsync(PostulacionDTO dto)
        {
            // Toda postulación nace Pendiente, sin importar el estado que llegue en el DTO.
            var postulacion = new Postulacion(0, dto.AlumnoId, dto.OfertaId, DateTime.Today, EstadoPostulacion.Pendiente);

            if (await alumnoRepository.GetAsync(dto.AlumnoId) == null)
                throw new ArgumentException("El alumno indicado no existe.");

            var oferta = await ofertaRepository.GetAsync(dto.OfertaId);
            if (oferta == null)
                throw new ArgumentException("La oferta indicada no existe.");

            if (oferta.Estado != EstadoOferta.Activa || oferta.FechaHasta.Date < DateTime.Today)
                throw new ArgumentException("La oferta no está disponible para postularse.");

            if (await postulacionRepository.ExistsAsync(dto.AlumnoId, dto.OfertaId))
                throw new ArgumentException("Ya existe una postulación de este alumno a esta oferta.");

            await postulacionRepository.AddAsync(postulacion);

            var creada = await postulacionRepository.GetAsync(postulacion.Id);
            return MapToDTO(creada ?? postulacion);
        }

        // Lo único que se puede modificar de una postulación es su estado.
        public async Task<bool> UpdateAsync(PostulacionDTO dto)
        {
            if (!Enum.TryParse(dto.Estado, true, out EstadoPostulacion estado)
                || !Enum.IsDefined(typeof(EstadoPostulacion), estado))
            {
                throw new ArgumentException("El estado indicado no es válido. Valores posibles: Pendiente, Aceptada o Rechazada.");
            }

            return await postulacionRepository.UpdateEstadoAsync(dto.Id, estado);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await postulacionRepository.DeleteAsync(id);
        }

        private static PostulacionDTO MapToDTO(Postulacion p)
        {
            return new PostulacionDTO
            {
                Id = p.Id,
                AlumnoId = p.AlumnoId,
                AlumnoNombre = p.Alumno == null ? null : $"{p.Alumno.ApeAlumno}, {p.Alumno.NomAlumno}",
                AlumnoLegajo = p.Alumno?.Legajo,
                AlumnoEmail = p.Alumno?.Email,
                AlumnoCarrera = p.Alumno?.Carrera?.NomCarrera,
                AlumnoPromedio = p.Alumno?.Promedio ?? 0,
                OfertaId = p.OfertaId,
                OfertaTitulo = p.Oferta?.Titulo,
                EmpresaId = p.Oferta?.EmpresaId ?? 0,
                EmpresaNombre = p.Oferta?.Empresa?.RazonSocial,
                FechaPostulacion = p.FechaPostulacion,
                Estado = p.Estado.ToString()
            };
        }
    }
}
