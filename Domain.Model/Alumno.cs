// Guía de estudio: Representa los datos personales y académicos de un estudiante y su relación con una carrera. Encapsula cambios mediante métodos Set y valida los valores recibidos.
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

        // Construye la entidad a partir de los datos recibidos y aplica sus reglas de inicialización.
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

        // Constructor sin parámetros utilizado por Entity Framework Core al materializar la entidad desde la base de datos.
        private Alumno() { }

        // Establece Id aplicando las comprobaciones definidas en este método.
        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        // Establece NomAlumno aplicando las comprobaciones definidas en este método.
        public void SetNomAlumno(string nomAlumno)
        {
            if (string.IsNullOrWhiteSpace(nomAlumno))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nomAlumno));
            NomAlumno = nomAlumno;
        }

        // Establece ApeAlumno aplicando las comprobaciones definidas en este método.
        public void SetApeAlumno(string apeAlumno)
        {
            if (string.IsNullOrWhiteSpace(apeAlumno))
                throw new ArgumentException("El apellido no puede ser nulo o vacío.", nameof(apeAlumno));
            ApeAlumno = apeAlumno;
        }

        // Establece Email aplicando las comprobaciones definidas en este método.
        public void SetEmail(string email)
        {
            if (!EsEmailValido(email))
                throw new ArgumentException("El email no tiene un formato válido.", nameof(email));
            Email = email;
        }

        // Comprueba que el correo no esté vacío y coincida con el formato esperado por la entidad.
        private static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // Establece Legajo aplicando las comprobaciones definidas en este método.
        public void SetLegajo(string legajo)
        {
            if (string.IsNullOrWhiteSpace(legajo))
                throw new ArgumentException("El legajo no puede ser nulo o vacío.", nameof(legajo));
            if (!Regex.IsMatch(legajo, @"^[0-9]+$"))
                throw new ArgumentException("El legajo debe contener solo números.", nameof(legajo));
            Legajo = legajo;
        }

        // Establece Dni aplicando las comprobaciones definidas en este método.
        public void SetDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede ser nulo o vacío.", nameof(dni));
            Dni = dni;
        }

        // Establece Plan aplicando las comprobaciones definidas en este método.
        public void SetPlan(string plan)
        {
            if (string.IsNullOrWhiteSpace(plan))
                throw new ArgumentException("El plan no puede ser nulo o vacío.", nameof(plan));
            Plan = plan;
        }

        // Establece AnioCurso aplicando las comprobaciones definidas en este método.
        public void SetAnioCurso(int anioCurso)
        {
            if (anioCurso <= 0)
                throw new ArgumentException("El año en curso debe ser mayor que 0.", nameof(anioCurso));
            AnioCurso = anioCurso;
        }

        // Establece CantMatAp aplicando las comprobaciones definidas en este método.
        public void SetCantMatAp(int cantMatAp)
        {
            if (cantMatAp < 0)
                throw new ArgumentException("La cantidad de materias aprobadas no puede ser negativa.", nameof(cantMatAp));
            CantMatAp = cantMatAp;
        }

        // Establece Promedio aplicando las comprobaciones definidas en este método.
        public void SetPromedio(float promedio)
        {
            if (promedio < 0 || promedio > 10)
                throw new ArgumentException("El promedio debe estar entre 0 y 10.", nameof(promedio));
            Promedio = promedio;
        }

        // Establece CarreraId aplicando las comprobaciones definidas en este método.
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

        // Establece Carrera aplicando las comprobaciones definidas en este método.
        public void SetCarrera(Carrera carrera)
        {
            ArgumentNullException.ThrowIfNull(carrera);
            _carrera = carrera;
            _carreraId = carrera.Id;
        }

        // Establece FechaAlta aplicando las comprobaciones definidas en este método.
        public void SetFechaAlta(DateTime fechaAlta)
        {
            if (fechaAlta == default)
                throw new ArgumentException("La fecha de alta no puede ser nula.", nameof(fechaAlta));
            FechaAlta = fechaAlta;
        }

    }
}
