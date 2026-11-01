namespace DTOs
{
    public class OfertaDTO : System.ComponentModel.DataAnnotations.IValidatableObject
    {
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(
            System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (FechaDesde == default)
                yield return new("Seleccione la fecha de inicio de la oferta.", new[] { nameof(FechaDesde) });
            if (FechaHasta == default)
                yield return new("Seleccione la fecha de finalización de la oferta.", new[] { nameof(FechaHasta) });
            if (FechaDesde != default && FechaHasta != default && FechaDesde > FechaHasta)
                yield return new("La fecha de finalización no puede ser anterior a la fecha de inicio.", new[] { nameof(FechaHasta) });
        }
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;

       
        public string Estado { get; set; } = string.Empty;

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Detalle { get; set; } = string.Empty;
        public List<RequisitoOfertaDTO> Requisitos { get; set; } = new(); public int EmpresaId { get; set; }
        public string? EmpresaNombre { get; set; }
        public int TipoOfertaId { get; set; }
        public string? TipoOfertaNombre { get; set; }
    }
}
