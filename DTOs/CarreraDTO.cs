using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class CarreraDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de la carrera es obligatorio.")]
        [StringLength(150, ErrorMessage = "El campo no puede superar los {1} caracteres.")]
        public string NomCarrera { get; set; } = string.Empty;
        [Required(ErrorMessage = "El departamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo no puede superar los {1} caracteres.")]
        public string Departamento { get; set; } = string.Empty;
        [Range(1, 10, ErrorMessage = "La duración debe estar entre 1 y 10 años.")]
        public int Duracion { get; set; }
    }
}
