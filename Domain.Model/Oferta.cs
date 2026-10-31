// Guía de estudio: Representa una oferta laboral con empresa, categoría, fechas, estado, vínculo y requisitos. Sus métodos Set permiten cambiar sus datos aplicando las reglas definidas en la entidad.
namespace Domain.Model
{
    public class Oferta
    {
        public int Id { get; private set; }
        public string Titulo { get; private set; }
        public TipoVinculo TipoVinculo { get; private set; }
        public DateTime FechaDesde { get; private set; }
        public DateTime FechaHasta { get; private set; }
        public string Detalle { get; private set; }
        public ICollection<RequisitoOferta> Requisitos { get; set; } = new List<RequisitoOferta>();
        public EstadoOferta Estado { get; private set; }

        private int _empresaId;
        private Empresa? _empresa;

        public int EmpresaId
        {
            get => _empresa?.Id ?? _empresaId;
            private set => _empresaId = value;
        }

        public Empresa? Empresa
        {
            get => _empresa;
            private set
            {
                _empresa = value;
                if (value != null && _empresaId != value.Id)
                    _empresaId = value.Id;
            }
        }

        private int _tipoOfertaId;
        private TipoOferta? _tipoOferta;

        public int TipoOfertaId
        {
            get => _tipoOferta?.Id ?? _tipoOfertaId;
            private set => _tipoOfertaId = value;
        }

        public TipoOferta? TipoOferta
        {
            get => _tipoOferta;
            private set
            {
                _tipoOferta = value;
                if (value != null && _tipoOfertaId != value.Id)
                    _tipoOfertaId = value.Id;
            }
        }

        // Construye la entidad a partir de los datos recibidos y aplica sus reglas de inicialización.
        public Oferta(int id, string titulo, TipoVinculo tipoVinculo, DateTime fechaDesde, DateTime fechaHasta,
                       string detalle, EstadoOferta estado, int empresaId, int tipoOfertaId)
        {
            SetId(id);
            SetTitulo(titulo);
            SetTipoVinculo(tipoVinculo);
            SetFechas(fechaDesde, fechaHasta);
            SetDetalle(detalle);
            SetEstado(estado);
            SetEmpresaId(empresaId);
            SetTipoOfertaId(tipoOfertaId);
        }

        // Constructor sin parámetros utilizado por Entity Framework Core al materializar la entidad desde la base de datos.
        private Oferta() { }

        // Establece Id aplicando las comprobaciones definidas en este método.
        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        // Establece Titulo aplicando las comprobaciones definidas en este método.
        public void SetTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título no puede ser nulo o vacío.", nameof(titulo));
            Titulo = titulo;
        }

        // Establece TipoVinculo aplicando las comprobaciones definidas en este método.
        public void SetTipoVinculo(TipoVinculo tipoVinculo)
        {
            if (!Enum.IsDefined(typeof(TipoVinculo), tipoVinculo))
                throw new ArgumentException("El tipo de vínculo no es válido.", nameof(tipoVinculo));
            TipoVinculo = tipoVinculo;
        }
        // Establece Fechas aplicando las comprobaciones definidas en este método.
        public void SetFechas(DateTime desde, DateTime hasta)
        {
            if (desde == default || hasta == default || desde > hasta)
                throw new ArgumentException("El período de la oferta debe tener fechas válidas y desde no puede superar hasta.");
            FechaDesde = desde;
            FechaHasta = hasta;
        }

        // Establece FechaDesde aplicando las comprobaciones definidas en este método.
        public void SetFechaDesde(DateTime fechaDesde)
        {
            if (fechaDesde == default)
                throw new ArgumentException("La fecha desde no puede ser nula.", nameof(fechaDesde));
            if (FechaHasta != default && fechaDesde > FechaHasta)
                throw new ArgumentException("La fecha desde no puede ser posterior a la fecha hasta.", nameof(fechaDesde));
            FechaDesde = fechaDesde;
        }

        // Establece FechaHasta aplicando las comprobaciones definidas en este método.
        public void SetFechaHasta(DateTime fechaHasta)
        {
            if (fechaHasta == default)
                throw new ArgumentException("La fecha hasta no puede ser nula.", nameof(fechaHasta));
            if (FechaDesde != default && fechaHasta < FechaDesde)
                throw new ArgumentException("La fecha hasta no puede ser anterior a la fecha desde.", nameof(fechaHasta));
            FechaHasta = fechaHasta;
        }

        // Establece Detalle aplicando las comprobaciones definidas en este método.
        public void SetDetalle(string detalle)
        {
            if (string.IsNullOrWhiteSpace(detalle))
                throw new ArgumentException("El detalle no puede ser nulo o vacío.", nameof(detalle));
            Detalle = detalle;
        }

        // Establece Requisitos aplicando las comprobaciones definidas en este método.
        public void SetRequisitos(ICollection<RequisitoOferta> nuevosRequisitos)
        {
            if (nuevosRequisitos == null)
                throw new ArgumentNullException(nameof(nuevosRequisitos), "La lista de requisitos no puede ser nula.");

            Requisitos = nuevosRequisitos;
        }

        // Establece Estado aplicando las comprobaciones definidas en este método.
        public void SetEstado(EstadoOferta estado)
        {
            if (!Enum.IsDefined(typeof(EstadoOferta), estado))
                throw new ArgumentException("El estado no es válido.", nameof(estado));
            Estado = estado;
        }

        // Establece EmpresaId aplicando las comprobaciones definidas en este método.
        public void SetEmpresaId(int empresaId)
        {
            if (empresaId <= 0)
                throw new ArgumentException("El EmpresaId debe ser mayor que 0.", nameof(empresaId));

            _empresaId = empresaId;
            if (_empresa != null && _empresa.Id != empresaId)
                _empresa = null;
        }

        // Establece Empresa aplicando las comprobaciones definidas en este método.
        public void SetEmpresa(Empresa empresa)
        {
            ArgumentNullException.ThrowIfNull(empresa);
            _empresa = empresa;
            _empresaId = empresa.Id;
        }

        // Establece TipoOfertaId aplicando las comprobaciones definidas en este método.
        public void SetTipoOfertaId(int tipoOfertaId)
        {
            if (tipoOfertaId <= 0)
                throw new ArgumentException("El TipoOfertaId debe ser mayor que 0.", nameof(tipoOfertaId));

            _tipoOfertaId = tipoOfertaId;
            if (_tipoOferta != null && _tipoOferta.Id != tipoOfertaId)
                _tipoOferta = null;
        }

        // Establece TipoOferta aplicando las comprobaciones definidas en este método.
        public void SetTipoOferta(TipoOferta tipoOferta)
        {
            ArgumentNullException.ThrowIfNull(tipoOferta);
            _tipoOferta = tipoOferta;
            _tipoOfertaId = tipoOferta.Id;
        }
    }
}
