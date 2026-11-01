using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class EmpresaRegistroRequest : EmpresaDTO, IValidatableObject
    {
        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required, Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((RazonSocial?.Trim().Length ?? 0) < 3)
                yield return new ValidationResult("La razón social debe tener al menos 3 caracteres.", new[] { nameof(RazonSocial) });
        }
    }

    public class EmpresaSolicitudDTO : EmpresaDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
