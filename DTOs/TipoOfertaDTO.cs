// Guía de estudio: Contrato de datos TipoOfertaDTO: agrupa los campos intercambiados entre API y clientes. Las propiedades describen el contenido de la solicitud o respuesta; no realizan acceso a base de datos.
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class TipoOfertaDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo no puede superar los {1} caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
