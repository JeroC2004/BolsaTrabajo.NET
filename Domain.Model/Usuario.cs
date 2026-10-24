using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Domain.Model
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Salt { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public bool Activo { get; private set; }
        public RolUsuario Rol { get; private set; }
        public int? AlumnoId { get; private set; }
        public int? EmpresaId { get; private set; }

        public Usuario(int id, string username, string email, string password, DateTime fechaCreacion, bool activo = true,
                       RolUsuario rol = RolUsuario.Administrador, int? alumnoId = null, int? empresaId = null)
        {
            SetId(id);
            SetUsername(username);
            SetEmail(email);
            SetPassword(password);
            SetFechaCreacion(fechaCreacion);
            SetActivo(activo);
            SetRol(rol, alumnoId, empresaId);
        }

        private Usuario() { }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El nombre de usuario no puede ser nulo o vacío.", nameof(username));
            if (username.Length < 3)
                throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres.", nameof(username));
            Username = username;
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

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede ser nula o vacía.", nameof(password));
            if (password.Length < 6)
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.", nameof(password));

            Salt = GenerateSalt();
            PasswordHash = HashPassword(password, Salt);
        }

        public void SetFechaCreacion(DateTime fechaCreacion)
        {
            if (fechaCreacion == default)
                throw new ArgumentException("La fecha de creación no puede ser nula.", nameof(fechaCreacion));
            FechaCreacion = fechaCreacion;
        }

        public void SetActivo(bool activo)
        {
            Activo = activo;
        }

        // Un usuario Alumno debe estar vinculado a un alumno y uno Empresa a una empresa;
        // el administrador no se vincula con ninguno.
        public void SetRol(RolUsuario rol, int? alumnoId = null, int? empresaId = null)
        {
            if (!Enum.IsDefined(typeof(RolUsuario), rol))
                throw new ArgumentException("El rol indicado no es válido.", nameof(rol));

            switch (rol)
            {
                case RolUsuario.Alumno:
                    if (alumnoId == null || alumnoId <= 0)
                        throw new ArgumentException("Un usuario con rol Alumno debe estar asociado a un alumno.", nameof(alumnoId));
                    empresaId = null;
                    break;

                case RolUsuario.Empresa:
                    if (empresaId == null || empresaId <= 0)
                        throw new ArgumentException("Un usuario con rol Empresa debe estar asociado a una empresa.", nameof(empresaId));
                    alumnoId = null;
                    break;

                default:
                    alumnoId = null;
                    empresaId = null;
                    break;
            }

            Rol = rol;
            AlumnoId = alumnoId;
            EmpresaId = empresaId;
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            string hashedInput = HashPassword(password, Salt);
            return PasswordHash == hashedInput;
        }

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
