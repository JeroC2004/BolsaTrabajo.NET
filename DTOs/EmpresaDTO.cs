using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class EmpresaDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [StringLength(150, ErrorMessage = "El campo no puede superar los {1} caracteres.")]
        public string RazonSocial { get; set; } = string.Empty;
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "El campo no puede superar los {1} caracteres.")]
        public string Descripcion { get; set; } = string.Empty;
        [Required(ErrorMessage = "El rubro es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo no puede superar los {1} caracteres.")]
        public string Rubro { get; set; } = string.Empty;
    }
}
