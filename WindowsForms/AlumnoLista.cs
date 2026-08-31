using API.Clients;
using DTOs;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class AlumnoLista : Form
    {
        private List<AlumnoDTO> alumnos = new();

        public AlumnoLista()
        {
            InitializeComponent();
        }

        private async void AlumnoLista_Load(object sender, EventArgs e)
        {
            await CargarAlumnosAsync();
        }

        private async Task CargarAlumnosAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var resultado = await AlumnoApiClient.GetAllAsync();
                alumnos = resultado.ToList();
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
                MessageBox.Show($"Error al cargar alumnos: {ex.Message}", "Error",
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
            dataGridView1.DataSource = alumnos.Select(a => new
            {
                a.Id,
                Nombre = a.NomAlumno,
                Apellido = a.ApeAlumno,
                a.Email,
                a.Legajo,
                a.Dni,
                Carrera = a.CarreraNombre,
                a.AnioCurso,
                a.Promedio
            }).ToList();
        }

        private async void buscarButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (string.IsNullOrWhiteSpace(buscarTextBox.Text))
                {
                    await CargarAlumnosAsync();
                    return;
                }

                var resultado = await AlumnoApiClient.GetByCriteriaAsync(buscarTextBox.Text);
                alumnos = resultado.ToList();
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
            await CargarAlumnosAsync();
        }

        private async void agregarButton_Click(object sender, EventArgs e)
        {
            AlumnoDetalle detalle = new AlumnoDetalle();
            if (detalle.ShowDialog() == DialogResult.OK)
            {
                await CargarAlumnosAsync();
            }
        }

        private async void actualizarButton_Click(object sender, EventArgs e)
        {
            var id = ObtenerIdSeleccionado();
            if (id == null)
            {
                MessageBox.Show("Seleccione un alumno de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AlumnoDetalle detalle = new AlumnoDetalle(id.Value);
            if (detalle.ShowDialog() == DialogResult.OK)
            {
                await CargarAlumnosAsync();
            }
        }

        private async void eliminarButton_Click(object sender, EventArgs e)
        {
            var id = ObtenerIdSeleccionado();
            if (id == null)
            {
                MessageBox.Show("Seleccione un alumno de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro que desea eliminar el alumno seleccionado?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                await AlumnoApiClient.DeleteAsync(id.Value);
                await CargarAlumnosAsync();
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
