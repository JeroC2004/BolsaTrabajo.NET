using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class AlumnoDTO
    {
        public int Id { get; set; }
        public string NomAlumno { get; set; } = string.Empty;
        public string ApeAlumno { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "El legajo es obligatorio.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El legajo debe contener solo números.")]
        public string Legajo { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;
        public int AnioCurso { get; set; }
        public int CantMatAp { get; set; }
        public float Promedio { get; set; }
        public int CarreraId { get; set; }
        public string? CarreraNombre { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
