using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(
            50,
            MinimumLength = 3,
            ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        [RegularExpression(
            @"^[a-zA-Z0-9._-]+$",
            ErrorMessage = "El usuario solo puede contener letras, números, puntos, guiones y guiones bajos.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string NomAlumno { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string ApeAlumno { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El legajo es obligatorio.")]
        public string Legajo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^\d{8,9}$",
            ErrorMessage = "El DNI debe contener únicamente entre 8 y 9 dígitos.")]
        public string Dni { get; set; } = string.Empty;

        [Required(ErrorMessage = "El plan es obligatorio.")]
        public string Plan { get; set; } = string.Empty;

        [Range(1, 5,
            ErrorMessage = "El año de curso debe estar entre 1 y 5.")]
        public int AnioCurso { get; set; }

        [Range(0, 99,
            ErrorMessage = "La cantidad de materias aprobadas debe estar entre 0 y 99.")]
        public int CantMatAp { get; set; }

        [Range(0, 10,
            ErrorMessage = "El promedio debe estar entre 0 y 10.")]
        public float Promedio { get; set; }

        [Range(1, 5,
            ErrorMessage = "Debe seleccionar una carrera válida.")]
        public int CarreraId { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(5,
            ErrorMessage = "La contraseña debe tener al menos 5 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [Compare(nameof(Password),
            ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}