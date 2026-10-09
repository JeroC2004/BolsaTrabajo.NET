using API.Clients;
using DTOs;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class AlumnoLista : Form
    {
        private List<AlumnoDTO> alumnos = new();

        private readonly AlumnoApiClient _alumnoClient;

        public AlumnoLista()
        {
            InitializeComponent();

            var httpClient = ApiHttpClientFactory.Shared;
            _alumnoClient = new AlumnoApiClient(httpClient);

            ConfigurarColumnas();
        }

        private void ConfigurarColumnas()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Legajo", HeaderText = "Legajo", Width = 90 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NomAlumno", HeaderText = "Nombre", Width = 130 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ApeAlumno", HeaderText = "Apellido", Width = 130 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Dni", HeaderText = "DNI", Width = 90 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 180 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CarreraNombre", HeaderText = "Carrera", Width = 220 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AnioCurso", HeaderText = "Año", Width = 60 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Promedio", HeaderText = "Promedio", Width = 80 });
        }

        private async void AlumnoLista_Load(object sender, EventArgs e) => await CargarAlumnosAsync();

        private async Task CargarAlumnosAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var resultado = await _alumnoClient.GetAllAsync();
                alumnos = resultado.ToList();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = alumnos;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión expiró. Vuelva a iniciar sesión.", "Sesión expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (string.IsNullOrWhiteSpace(buscarTextBox.Text)) { await CargarAlumnosAsync(); return; }

                var resultado = await _alumnoClient.GetByCriteriaAsync(buscarTextBox.Text);
                alumnos = resultado.ToList();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = alumnos;
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
            await CargarAlumnosAsync();
        }

        private async void agregarButton_Click(object sender, EventArgs e)
        {
            var detalle = new AlumnoDetalle();
            if (detalle.ShowDialog(this) == DialogResult.OK) await CargarAlumnosAsync();
        }

        private async void actualizarButton_Click(object sender, EventArgs e)
        {
            var seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show("Seleccione un alumno de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var detalle = new AlumnoDetalle(seleccionado.Id);
            if (detalle.ShowDialog(this) == DialogResult.OK) await CargarAlumnosAsync();
        }

        private async void eliminarButton_Click(object sender, EventArgs e)
        {
            var seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show("Seleccione un alumno de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro que desea eliminar el alumno seleccionado?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                await _alumnoClient.DeleteAsync(seleccionado.Id);
                await CargarAlumnosAsync();
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

        private AlumnoDTO? ObtenerSeleccionado() => dataGridView1.CurrentRow?.DataBoundItem as AlumnoDTO;
    }
}