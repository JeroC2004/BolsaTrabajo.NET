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
        public string Requisitos { get; private set; }
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

        public Oferta(int id, string titulo, TipoVinculo tipoVinculo, DateTime fechaDesde, DateTime fechaHasta,
                       string detalle, string requisitos, EstadoOferta estado, int empresaId, int tipoOfertaId)
        {
            SetId(id);
            SetTitulo(titulo);
            SetTipoVinculo(tipoVinculo);
            // FechaDesde y FechaHasta se validan cruzadas entre sí: se asignan directamente
            // primero y se revalidan juntas al final del constructor (ver nota en SetFechaDesde).
            FechaDesde = fechaDesde;
            SetFechaHasta(fechaHasta);
            SetFechaDesde(fechaDesde);
            SetDetalle(detalle);
            SetRequisitos(requisitos);
            SetEstado(estado);
            SetEmpresaId(empresaId);
            SetTipoOfertaId(tipoOfertaId);
        }

        private Oferta() { } // Constructor privado requerido por EF Core

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

        public void SetTipoVinculo(TipoVinculo tipoVinculo)
        {
            if (!Enum.IsDefined(typeof(TipoVinculo), tipoVinculo))
                throw new ArgumentException("El tipo de vínculo no es válido.", nameof(tipoVinculo));
            TipoVinculo = tipoVinculo;
        }

        // Corrección de la Entrega 1: antes SetFechaDesde no revalidaba contra FechaHasta,
        // por lo que una modificación podía dejar el rango invertido pasando primero por este setter.
        // Ahora ambos setters se validan cruzados entre sí en los dos sentidos.
        public void SetFechaDesde(DateTime fechaDesde)
        {
            if (fechaDesde == default)
                throw new ArgumentException("La fecha desde no puede ser nula.", nameof(fechaDesde));
            if (FechaHasta != default && fechaDesde > FechaHasta)
                throw new ArgumentException("La fecha desde no puede ser posterior a la fecha hasta.", nameof(fechaDesde));
            FechaDesde = fechaDesde;
        }

        public void SetFechaHasta(DateTime fechaHasta)
        {
            if (fechaHasta == default)
                throw new ArgumentException("La fecha hasta no puede ser nula.", nameof(fechaHasta));
            if (FechaDesde != default && fechaHasta < FechaDesde)
                throw new ArgumentException("La fecha hasta no puede ser anterior a la fecha desde.", nameof(fechaHasta));
            FechaHasta = fechaHasta;
        }

        public void SetDetalle(string detalle)
        {
            if (string.IsNullOrWhiteSpace(detalle))
                throw new ArgumentException("El detalle no puede ser nulo o vacío.", nameof(detalle));
            Detalle = detalle;
        }

        public void SetRequisitos(string requisitos)
        {
            if (string.IsNullOrWhiteSpace(requisitos))
                throw new ArgumentException("Los requisitos no pueden ser nulos o vacíos.", nameof(requisitos));
            Requisitos = requisitos;
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

        public void SetEmpresa(Empresa empresa)
        {
            ArgumentNullException.ThrowIfNull(empresa);
            _empresa = empresa;
            _empresaId = empresa.Id;
        }

        public void SetTipoOfertaId(int tipoOfertaId)
        {
            if (tipoOfertaId <= 0)
                throw new ArgumentException("El TipoOfertaId debe ser mayor que 0.", nameof(tipoOfertaId));

            _tipoOfertaId = tipoOfertaId;
            if (_tipoOferta != null && _tipoOferta.Id != tipoOfertaId)
                _tipoOferta = null;
        }

        public void SetTipoOferta(TipoOferta tipoOferta)
        {
            ArgumentNullException.ThrowIfNull(tipoOferta);
            _tipoOferta = tipoOferta;
            _tipoOfertaId = tipoOferta.Id;
        }
    }
}
