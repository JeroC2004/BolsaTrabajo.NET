using System.Text.RegularExpressions;

namespace Domain.Model
{
    public class Alumno
    {
        public int Id { get; private set; }
        public string NomAlumno { get; private set; }
        public string ApeAlumno { get; private set; }
        public string Email { get; private set; }
        public string Legajo { get; private set; }
        public string Dni { get; private set; }
        public string Plan { get; private set; }
        public int AnioCurso { get; private set; }
        public int CantMatAp { get; private set; }
        public float Promedio { get; private set; }

        private int _carreraId;
        private Carrera? _carrera;

        public int CarreraId
        {
            get => _carrera?.Id ?? _carreraId;
            private set => _carreraId = value;
        }

        public Carrera? Carrera
        {
            get => _carrera;
            private set
            {
                _carrera = value;
                if (value != null && _carreraId != value.Id)
                {
                    _carreraId = value.Id;
                }
            }
        }

        public DateTime FechaAlta { get; private set; }

        public Alumno(int id, string nomAlumno, string apeAlumno, string email, string legajo, string dni,
                       string plan, int anioCurso, int cantMatAp, float promedio, int carreraId, DateTime fechaAlta)
        {
            SetId(id);
            SetNomAlumno(nomAlumno);
            SetApeAlumno(apeAlumno);
            SetEmail(email);
            SetLegajo(legajo);
            SetDni(dni);
            SetPlan(plan);
            SetAnioCurso(anioCurso);
            SetCantMatAp(cantMatAp);
            SetPromedio(promedio);
            SetCarreraId(carreraId);
            SetFechaAlta(fechaAlta);
        }

        private Alumno() { } // Constructor privado requerido por EF Core

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        public void SetNomAlumno(string nomAlumno)
        {
            if (string.IsNullOrWhiteSpace(nomAlumno))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nomAlumno));
            NomAlumno = nomAlumno;
        }

        public void SetApeAlumno(string apeAlumno)
        {
            if (string.IsNullOrWhiteSpace(apeAlumno))
                throw new ArgumentException("El apellido no puede ser nulo o vacío.", nameof(apeAlumno));
            ApeAlumno = apeAlumno;
        }

        public void SetEmail(string email)
        {
            if (!EsEmailValido(email))
                throw new ArgumentException("El email no tiene un formato válido.", nameof(email));
            Email = email;
        }

        private static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public void SetLegajo(string legajo)
        {
            if (string.IsNullOrWhiteSpace(legajo))
                throw new ArgumentException("El legajo no puede ser nulo o vacío.", nameof(legajo));
            Legajo = legajo;
        }

        public void SetDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede ser nulo o vacío.", nameof(dni));
            Dni = dni;
        }

        public void SetPlan(string plan)
        {
            if (string.IsNullOrWhiteSpace(plan))
                throw new ArgumentException("El plan no puede ser nulo o vacío.", nameof(plan));
            Plan = plan;
        }

        public void SetAnioCurso(int anioCurso)
        {
            if (anioCurso <= 0)
                throw new ArgumentException("El año en curso debe ser mayor que 0.", nameof(anioCurso));
            AnioCurso = anioCurso;
        }

        public void SetCantMatAp(int cantMatAp)
        {
            if (cantMatAp < 0)
                throw new ArgumentException("La cantidad de materias aprobadas no puede ser negativa.", nameof(cantMatAp));
            CantMatAp = cantMatAp;
        }

        public void SetPromedio(float promedio)
        {
            if (promedio < 0 || promedio > 10)
                throw new ArgumentException("El promedio debe estar entre 0 y 10.", nameof(promedio));
            Promedio = promedio;
        }

        public void SetCarreraId(int carreraId)
        {
            if (carreraId <= 0)
                throw new ArgumentException("El CarreraId debe ser mayor que 0.", nameof(carreraId));

            _carreraId = carreraId;

            if (_carrera != null && _carrera.Id != carreraId)
            {
                _carrera = null;
            }
        }

        public void SetCarrera(Carrera carrera)
        {
            ArgumentNullException.ThrowIfNull(carrera);
            _carrera = carrera;
            _carreraId = carrera.Id;
        }

        public void SetFechaAlta(DateTime fechaAlta)
        {
            if (fechaAlta == default)
                throw new ArgumentException("La fecha de alta no puede ser nula.", nameof(fechaAlta));
            FechaAlta = fechaAlta;
        }
    }
}
