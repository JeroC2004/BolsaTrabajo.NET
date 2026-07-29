using Domain.Model;
using Data;
using DTOs;

namespace Application.Services
{
    public class AlumnoService : IAlumnoService
    {
        private readonly IAlumnoRepository alumnoRepository;

        public AlumnoService(IAlumnoRepository alumnoRepository)
        {
            this.alumnoRepository = alumnoRepository;
        }

        public async Task<AlumnoDTO> AddAsync(AlumnoDTO dto)
        {
            if (await alumnoRepository.EmailExistsAsync(dto.Email))
                throw new ArgumentException($"Ya existe un alumno con el Email '{dto.Email}'.");

            if (await alumnoRepository.LegajoExistsAsync(dto.Legajo))
                throw new ArgumentException($"Ya existe un alumno con el Legajo '{dto.Legajo}'.");

            var fechaAlta = DateTime.Now;
            Alumno alumno = new Alumno(0, dto.NomAlumno, dto.ApeAlumno, dto.Email, dto.Legajo, dto.Dni,
                                        dto.Plan, dto.AnioCurso, dto.CantMatAp, dto.Promedio, dto.CarreraId, fechaAlta);

            await alumnoRepository.AddAsync(alumno);

            dto.Id = alumno.Id;
            dto.FechaAlta = alumno.FechaAlta;
            dto.CarreraNombre = alumno.Carrera?.NomCarrera;

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await alumnoRepository.DeleteAsync(id);
        }

        public async Task<AlumnoDTO?> GetAsync(int id)
        {
            Alumno? alumno = await alumnoRepository.GetAsync(id);

            if (alumno == null)
                return null;

            return MapToDTO(alumno);
        }

        public async Task<IEnumerable<AlumnoDTO>> GetAllAsync()
        {
            var alumnos = await alumnoRepository.GetAllAsync();
            return alumnos.Select(MapToDTO).ToList();
        }

        public async Task<bool> UpdateAsync(AlumnoDTO dto)
        {
            if (await alumnoRepository.EmailExistsAsync(dto.Email, dto.Id))
                throw new ArgumentException($"Ya existe otro alumno con el Email '{dto.Email}'.");

            if (await alumnoRepository.LegajoExistsAsync(dto.Legajo, dto.Id))
                throw new ArgumentException($"Ya existe otro alumno con el Legajo '{dto.Legajo}'.");

            var existing = await alumnoRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            Alumno alumno = new Alumno(dto.Id, dto.NomAlumno, dto.ApeAlumno, dto.Email, dto.Legajo, dto.Dni,
                                        dto.Plan, dto.AnioCurso, dto.CantMatAp, dto.Promedio, dto.CarreraId, existing.FechaAlta);

            return await alumnoRepository.UpdateAsync(alumno);
        }

        public async Task<IEnumerable<AlumnoDTO>> GetByCriteriaAsync(AlumnoCriteriaDTO criteriaDTO)
        {
            var criteria = new AlumnoCriteria(criteriaDTO.Texto);
            var alumnos = await alumnoRepository.GetByCriteriaAsync(criteria);
            return alumnos.Select(MapToDTO).ToList();
        }

        private static AlumnoDTO MapToDTO(Alumno alumno)
        {
            return new AlumnoDTO
            {
                Id = alumno.Id,
                NomAlumno = alumno.NomAlumno,
                ApeAlumno = alumno.ApeAlumno,
                Email = alumno.Email,
                Legajo = alumno.Legajo,
                Dni = alumno.Dni,
                Plan = alumno.Plan,
                AnioCurso = alumno.AnioCurso,
                CantMatAp = alumno.CantMatAp,
                Promedio = alumno.Promedio,
                CarreraId = alumno.CarreraId,
                CarreraNombre = alumno.Carrera?.NomCarrera,
                FechaAlta = alumno.FechaAlta
            };
        }
    }
}
