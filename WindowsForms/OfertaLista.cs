using API.Clients;
using DTOs;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class OfertaLista : Form
    {
        private List<OfertaDTO> ofertas = new();

        public OfertaLista()
        {
            InitializeComponent();
        }

        private async void OfertaLista_Load(object sender, EventArgs e)
        {
            await CargarOfertasAsync();
        }

        private async Task CargarOfertasAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var resultado = await OfertaApiClient.GetAllAsync();
                ofertas = resultado.ToList();
                RefrescarGrilla();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión expiró. Vuelva a iniciar sesión.", "Sesión expirada",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ofertas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void RefrescarGrilla()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ofertas.Select(o => new
            {
                o.Id,
                o.Titulo,
                Empresa = o.EmpresaNombre,
                Tipo = o.TipoOfertaNombre,
                o.TipoVinculo,
                o.Estado,
                Desde = o.FechaDesde.ToShortDateString(),
                Hasta = o.FechaHasta.ToShortDateString()
            }).ToList();
        }

        private async void buscarButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (string.IsNullOrWhiteSpace(buscarTextBox.Text))
                {
                    await CargarOfertasAsync();
                    return;
                }

                var resultado = await OfertaApiClient.GetByCriteriaAsync(buscarTextBox.Text);
                ofertas = resultado.ToList();
                RefrescarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            OfertaDetalle detalle = new OfertaDetalle();
            if (detalle.ShowDialog() == DialogResult.OK)
            {
                await CargarOfertasAsync();
            }
        }

        private async void actualizarButton_Click(object sender, EventArgs e)
        {
            var id = ObtenerIdSeleccionado();
            if (id == null)
            {
                MessageBox.Show("Seleccione una oferta de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OfertaDetalle detalle = new OfertaDetalle(id.Value);
            if (detalle.ShowDialog() == DialogResult.OK)
            {
                await CargarOfertasAsync();
            }
        }

        private async void eliminarButton_Click(object sender, EventArgs e)
        {
            var id = ObtenerIdSeleccionado();
            if (id == null)
            {
                MessageBox.Show("Seleccione una oferta de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro que desea eliminar la oferta seleccionada?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                await OfertaApiClient.DeleteAsync(id.Value);
                await CargarOfertasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                actualizarButton_Click(sender, e);
        }

        private int? ObtenerIdSeleccionado()
        {
            if (dataGridView1.CurrentRow == null)
                return null;

            var cell = dataGridView1.CurrentRow.Cells["Id"];
            if (cell?.Value == null)
                return null;

            return Convert.ToInt32(cell.Value);
        }
    }
}
