using API.Clients;
using DTOs;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class OfertaLista : Form
    {
        private List<OfertaDTO> ofertas = new();

        private readonly OfertaApiClient _ofertaClient;

        public OfertaLista()
        {
            InitializeComponent();

            var httpClient = ApiHttpClientFactory.Shared;
            _ofertaClient = new OfertaApiClient(httpClient);

            ConfigurarColumnas();
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

        private async void OfertaLista_Load(object sender, EventArgs e) => await CargarOfertasAsync();

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
                if (string.IsNullOrWhiteSpace(buscarTextBox.Text)) { await CargarOfertasAsync(); return; }

                var resultado = await _ofertaClient.GetByCriteriaAsync(buscarTextBox.Text);
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
            if (e.RowIndex >= 0) actualizarButton_Click(sender, e);
        }

        private OfertaDTO? ObtenerSeleccionada() => dataGridView1.CurrentRow?.DataBoundItem as OfertaDTO;
    }
}