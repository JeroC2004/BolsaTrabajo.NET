namespace Domain.Model
{
    public class Oferta
    {
        public int Id { get; private set; }
        public string Titulo { get; private set; }
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

        public Oferta(int id, string titulo, DateTime fechaDesde, DateTime fechaHasta,
                       string detalle, EstadoOferta estado, int empresaId, int tipoOfertaId)
        {
            SetId(id);
            SetTitulo(titulo);
            SetFechas(fechaDesde, fechaHasta);
            SetDetalle(detalle);
            SetEstado(estado);
            SetEmpresaId(empresaId);
            SetTipoOfertaId(tipoOfertaId);
        }

        private Oferta() { }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        public void SetTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título no puede ser nulo o vacío.", nameof(titulo));
            Titulo = titulo;
        }

        public void SetFechas(DateTime desde, DateTime hasta)
        {
            if (desde == default)
                throw new ArgumentException("Seleccione la fecha de inicio de la oferta.");
            if (hasta == default)
                throw new ArgumentException("Seleccione la fecha de finalización de la oferta.");
            if (desde > hasta)
                throw new ArgumentException("La fecha de finalización no puede ser anterior a la fecha de inicio.");
            FechaDesde = desde;
            FechaHasta = hasta;
        }

        public void SetDetalle(string detalle)
        {
            if (string.IsNullOrWhiteSpace(detalle))
                throw new ArgumentException("El detalle no puede ser nulo o vacío.", nameof(detalle));
            Detalle = detalle;
        }

        public void SetRequisitos(ICollection<RequisitoOferta> nuevosRequisitos)
        {
            if (nuevosRequisitos == null)
                throw new ArgumentNullException(nameof(nuevosRequisitos), "La lista de requisitos no puede ser nula.");

            Requisitos = nuevosRequisitos;
        }

        public void SetEstado(EstadoOferta estado)
        {
            if (!Enum.IsDefined(typeof(EstadoOferta), estado))
                throw new ArgumentException("El estado no es válido.", nameof(estado));
            Estado = estado;
        }

        public void SetEmpresaId(int empresaId)
        {
            if (empresaId <= 0)
                throw new ArgumentException("El EmpresaId debe ser mayor que 0.", nameof(empresaId));

            _empresaId = empresaId;
            if (_empresa != null && _empresa.Id != empresaId)
                _empresa = null;
        }

        public void SetTipoOfertaId(int tipoOfertaId)
        {
            if (tipoOfertaId <= 0)
                throw new ArgumentException("El TipoOfertaId debe ser mayor que 0.", nameof(tipoOfertaId));

            _tipoOfertaId = tipoOfertaId;
            if (_tipoOferta != null && _tipoOferta.Id != tipoOfertaId)
                _tipoOferta = null;
        }
    }
}
