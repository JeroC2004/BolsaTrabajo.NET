using API.Clients;
using Domain.Model;
using DTOs;

namespace WindowsForms
{
    internal sealed class OfertaConsulta : Form
    {
        private readonly int ofertaId;
        private readonly OfertaApiClient ofertaClient = new(ApiHttpClientFactory.Shared);
        private readonly PostulacionApiClient postulacionClient = new(ApiHttpClientFactory.Shared);
        private readonly Label titulo = new() { AutoSize = true, Text = "Cargando oferta..." };
        private readonly Label informacion = new() { AutoSize = true };
        private readonly TextBox descripcion = new()
        {
            Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill
        };
        private readonly DataGridView requisitos = new()
        {
            Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = false,
            AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersVisible = false,
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        };
        private readonly Label mensaje = new() { AutoSize = true };
        private readonly Button postularme = new() { Text = "Postularme", AutoSize = true, Enabled = false };
        private OfertaDTO? oferta;
        private bool postulado;
        private bool enviando;
        private bool Disponible => oferta?.Estado == nameof(EstadoOferta.Activa)
            && oferta.FechaHasta.Date >= DateTime.Today;

        public OfertaConsulta(int ofertaId)
        {
            this.ofertaId = ofertaId;
            Text = "Detalle de oferta";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(760, 720);
            MinimumSize = new Size(600, 500);
            MinimizeBox = false;
            ShowInTaskbar = false;
            titulo.Font = new Font(Font.FontFamily, 14, FontStyle.Bold);
            requisitos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            requisitos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descripcion", HeaderText = "Requisito",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            requisitos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Condicion", HeaderText = "Condición", Width = 140
            });

            var cerrar = new Button { Text = "Cerrar", AutoSize = true, DialogResult = DialogResult.Cancel };
            var botones = new FlowLayoutPanel
            {
                AutoSize = true, Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft
            };
            botones.Controls.Add(cerrar);
            botones.Controls.Add(postularme);
            postularme.Visible = Sesion.EsAlumno;
            postularme.Click += PostularseAsync;
            CancelButton = cerrar;

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 8, Padding = new Padding(12) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            Control[] contenido =
            {
                titulo, informacion, new Label { Text = "Descripción del puesto", AutoSize = true }, descripcion,
                new Label { Text = "Requisitos", AutoSize = true }, requisitos, mensaje, botones
            };
            for (int fila = 0; fila < contenido.Length; fila++)
            {
                layout.RowStyles.Add(fila == 3 || fila == 5
                    ? new RowStyle(SizeType.Percent, fila == 3 ? 40 : 60)
                    : new RowStyle(SizeType.AutoSize));
                layout.Controls.Add(contenido[fila], 0, fila);
            }
            Controls.Add(layout);
            Shown += CargarAsync;
        }

        private async void CargarAsync(object? sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var resultado = await ofertaClient.GetAsync(ofertaId);
                bool yaPostulado = Sesion.EsAlumno && (await postulacionClient.GetAllAsync(ofertaId)).Any();
                if (IsDisposed) return;
                oferta = resultado;
                postulado = yaPostulado;
                titulo.Text = oferta.Titulo;
                informacion.Text = $"Empresa: {oferta.EmpresaNombre}\nTipo de oferta: {oferta.TipoOfertaNombre}\n" +
                    $"Vínculo: {oferta.TipoVinculo}    Estado: {oferta.Estado}\n" +
                    $"Desde: {oferta.FechaDesde:dd/MM/yyyy}    Hasta: {oferta.FechaHasta:dd/MM/yyyy}";
                descripcion.Text = oferta.Detalle;
                requisitos.DataSource = oferta.Requisitos.Select(r => new
                {
                    r.Descripcion, Condicion = r.EsExcluyente ? "Excluyente" : "No excluyente"
                }).ToList();
                mensaje.Text = oferta.Requisitos.Count == 0 ? "La empresa no especificó requisitos adicionales." : string.Empty;
                if (Sesion.EsAlumno)
                {
                    if (postulado) mensaje.Text = "Ya estás postulado a esta oferta. Consultá Mis postulaciones para ver el estado.";
                    else if (!Disponible) mensaje.Text = "Esta oferta no está disponible para postularse.";
                    postularme.Enabled = !postulado && Disponible;
                }
            }
            catch (Exception ex)
            {
                if (IsDisposed) return;
                titulo.Text = "No se pudo cargar la oferta";
                mensaje.Text = ex is UnauthorizedAccessException ? "Su sesión expiró. Vuelva a iniciar sesión." : ex.Message;
            }
            finally
            {
                if (!IsDisposed) Cursor = Cursors.Default;
            }
        }

        private async void PostularseAsync(object? sender, EventArgs e)
        {
            if (enviando || postulado || !Disponible) return;
            enviando = true;
            postularme.Enabled = false;
            try
            {
                await postulacionClient.AddAsync(new PostulacionDTO { OfertaId = ofertaId });
                postulado = true;
                if (!IsDisposed) mensaje.Text = "Te postulaste correctamente. Consultá Mis postulaciones para ver el estado.";
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                    mensaje.Text = ex is UnauthorizedAccessException ? "Su sesión expiró. Vuelva a iniciar sesión." : ex.Message;
            }
            finally
            {
                enviando = false;
                if (!IsDisposed) postularme.Enabled = !postulado && Disponible;
            }
        }
    }
}
