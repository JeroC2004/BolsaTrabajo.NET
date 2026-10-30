using API.Clients;
using DTOs;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class OfertaLista : Form
    {
        private List<OfertaDTO> ofertas = new();
        private readonly ComboBox tipoFiltro = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 180 };
        private readonly ComboBox estadoFiltro = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160 };
        private readonly ComboBox vinculoFiltro = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 180 };
        private readonly DateTimePicker desdeFiltro = new() { ShowCheckBox = true, Checked = false, Format = DateTimePickerFormat.Short, Width = 145 };
        private readonly DateTimePicker hastaFiltro = new() { ShowCheckBox = true, Checked = false, Format = DateTimePickerFormat.Short, Width = 145 };


        private readonly OfertaApiClient _ofertaClient;
        private readonly Button verDetalleButton = new();

        public OfertaLista()
        {
            InitializeComponent();

            var httpClient = ApiHttpClientFactory.Shared;
            _ofertaClient = new OfertaApiClient(httpClient);

            ConfigurarColumnas();
            AplicarPermisos();
            ConfigurarFiltros();
        }

        private void ConfigurarFiltros()
        {
            estadoFiltro.Items.Add("Todos los estados");
            estadoFiltro.Items.AddRange(Sesion.EsAlumno ? new object[] { "Activa" } : Enum.GetNames<Domain.Model.EstadoOferta>().Cast<object>().ToArray());
            vinculoFiltro.Items.Add("Todos los vínculos");
            vinculoFiltro.Items.AddRange(Enum.GetNames<Domain.Model.TipoVinculo>().Cast<object>().ToArray());
            estadoFiltro.SelectedIndex = vinculoFiltro.SelectedIndex = 0;
            tipoFiltro.DisplayMember = nameof(TipoOfertaDTO.Nombre);
            tipoFiltro.ValueMember = nameof(TipoOfertaDTO.Id);
            var barra = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, Padding = new Padding(8) };
            foreach (var control in new Control[] { verDetalleButton, agregarButton, actualizarButton, eliminarButton, buscarTextBox, buscarButton, refrescarButton })
                barra.Controls.Add(control);
            var filtros = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, Padding = new Padding(8) };
            void Filtro(string nombre, Control control)
            {
                filtros.Controls.Add(new Label { Text = nombre, AutoSize = true, Margin = new Padding(4, 8, 4, 4) });
                filtros.Controls.Add(control);
            }
            Filtro("Tipo", tipoFiltro); Filtro("Estado", estadoFiltro); Filtro("Vínculo", vinculoFiltro);
            Filtro("Publicada desde", desdeFiltro); Filtro("hasta", hastaFiltro);
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            dataGridView1.Dock = DockStyle.Fill;
            layout.Controls.Add(barra, 0, 0); layout.Controls.Add(filtros, 0, 1); layout.Controls.Add(dataGridView1, 0, 2);
            Controls.Add(layout);
        }

        private static bool PuedeGestionar => Sesion.EsAdministrador || Sesion.EsEmpresa;

        private void AplicarPermisos()
        {
            agregarButton.Visible = PuedeGestionar;
            actualizarButton.Visible = PuedeGestionar;
            eliminarButton.Visible = PuedeGestionar;

            verDetalleButton.Text = "Ver detalle";
            verDetalleButton.AutoSize = true;
            verDetalleButton.UseVisualStyleBackColor = true;
            verDetalleButton.Click += verDetalleButton_Click;
        }

        private void verDetalleButton_Click(object? sender, EventArgs e)
        {
            var seleccionada = ObtenerSeleccionada();
            if (seleccionada == null)
            {
                MessageBox.Show("Seleccione una oferta de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var consulta = new OfertaConsulta(seleccionada.Id);
            consulta.ShowDialog(this);
        }

        private void ConfigurarColumnas()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Titulo", HeaderText = "Título", Width = 200 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EmpresaNombre", HeaderText = "Empresa", Width = 180 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TipoOfertaNombre", HeaderText = "Tipo de Oferta", Width = 160 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TipoVinculo", HeaderText = "Vínculo", Width = 130 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Estado", HeaderText = "Estado", Width = 90 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaDesde",
                HeaderText = "Desde",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaHasta",
                HeaderText = "Hasta",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
        }

        private async void OfertaLista_Load(object sender, EventArgs e)
        {
            try
            {
                var tipos = (await new TipoOfertaApiClient(ApiHttpClientFactory.Shared).GetAllAsync()).ToList();
                tipos.Insert(0, new TipoOfertaDTO { Id = 0, Nombre = "Todos los tipos" });
                tipoFiltro.DataSource = tipos;
            }
            catch (Exception ex) { MessageBox.Show(this, "No se pudieron cargar los tipos: " + ex.Message); }
            await CargarOfertasAsync();
        }

        private async Task CargarOfertasAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var resultado = await _ofertaClient.GetAllAsync();
                ofertas = resultado.ToList();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = ofertas;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión expiró. Vuelva a iniciar sesión.", "Sesión expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ofertas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void buscarButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (desdeFiltro.Checked && hastaFiltro.Checked && desdeFiltro.Value.Date > hastaFiltro.Value.Date)
                    throw new ArgumentException("La fecha desde no puede superar a la fecha hasta.");
                var criteria = new OfertaCriteriaDTO
                {
                    Texto = buscarTextBox.Text,
                    TipoOfertaId = tipoFiltro.SelectedValue is int id && id > 0 ? id : null,
                    Estado = estadoFiltro.SelectedIndex > 0 ? estadoFiltro.Text : null,
                    TipoVinculo = vinculoFiltro.SelectedIndex > 0 ? vinculoFiltro.Text : null,
                    Desde = desdeFiltro.Checked ? desdeFiltro.Value.Date : null,
                    Hasta = hastaFiltro.Checked ? hastaFiltro.Value.Date : null
                };
                var resultado = await _ofertaClient.GetByCriteriaAsync(criteria);
                ofertas = resultado.ToList();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = ofertas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void refrescarButton_Click(object sender, EventArgs e)
        {
            buscarTextBox.Clear();
            if (tipoFiltro.Items.Count > 0) tipoFiltro.SelectedIndex = 0;
            estadoFiltro.SelectedIndex = vinculoFiltro.SelectedIndex = 0;
            desdeFiltro.Checked = hastaFiltro.Checked = false;
            await CargarOfertasAsync();
        }

        private async void agregarButton_Click(object sender, EventArgs e)
        {
            var detalle = new OfertaDetalle();
            if (detalle.ShowDialog(this) == DialogResult.OK) await CargarOfertasAsync();
        }

        private async void actualizarButton_Click(object sender, EventArgs e)
        {
            var seleccionada = ObtenerSeleccionada();
            if (seleccionada == null)
            {
                MessageBox.Show("Seleccione una oferta de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var detalle = new OfertaDetalle(seleccionada.Id);
            if (detalle.ShowDialog(this) == DialogResult.OK) await CargarOfertasAsync();
        }

        private async void eliminarButton_Click(object sender, EventArgs e)
        {
            var seleccionada = ObtenerSeleccionada();
            if (seleccionada == null)
            {
                MessageBox.Show("Seleccione una oferta de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro que desea eliminar la oferta seleccionada?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                await _ofertaClient.DeleteAsync(seleccionada.Id);
                await CargarOfertasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) verDetalleButton_Click(sender, e);
        }

        private OfertaDTO? ObtenerSeleccionada() => dataGridView1.CurrentRow?.DataBoundItem as OfertaDTO;
    }
}
