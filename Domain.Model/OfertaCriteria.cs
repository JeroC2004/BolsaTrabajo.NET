namespace Domain.Model
{
    public class OfertaCriteria
    {
        public string Texto { get; }
        public int? TipoOfertaId { get; }
        public EstadoOferta? Estado { get; }
        public TipoVinculo? TipoVinculo { get; }
        public DateTime? Desde { get; }
        public DateTime? Hasta { get; }

        public OfertaCriteria(string? texto, int? tipoOfertaId = null, EstadoOferta? estado = null,
            TipoVinculo? tipoVinculo = null, DateTime? desde = null, DateTime? hasta = null)
        {
            if (tipoOfertaId.HasValue && tipoOfertaId.Value <= 0)
                throw new ArgumentException("El tipo de oferta debe ser válido.");
            if (estado.HasValue && !Enum.IsDefined(typeof(EstadoOferta), estado.Value))
                throw new ArgumentException("El estado no es válido.");
            if (tipoVinculo.HasValue && !Enum.IsDefined(typeof(TipoVinculo), tipoVinculo.Value))
                throw new ArgumentException("El vínculo no es válido.");
            if (desde.HasValue && hasta.HasValue && desde.Value.Date > hasta.Value.Date)
                throw new ArgumentException("La fecha desde no puede superar a la fecha hasta.");
            Texto = texto?.Trim() ?? string.Empty;
            TipoOfertaId = tipoOfertaId;
            Estado = estado;
            TipoVinculo = tipoVinculo;
            Desde = desde?.Date;
            Hasta = hasta?.Date;
        }
    }
}
