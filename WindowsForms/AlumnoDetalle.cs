using API.Clients;
using DTOs;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class AlumnoDetalle : Form
    {
        private readonly int? alumnoId;
        private List<CarreraDTO> carreras = new();

        // Constructor sin parámetros: modo alta
        public AlumnoDetalle()
        {
            InitializeComponent();
            alumnoId = null;
        }

        // Constructor con id: modo edición
        public AlumnoDetalle(int id)
        {
            InitializeComponent();
            alumnoId = id;
        }

        private async void AlumnoDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var resultado = await CarreraApiClient.GetAllAsync();
                carreras = resultado.ToList();
                carreraComboBox.DataSource = carreras;
                carreraComboBox.DisplayMember = "NomCarrera";
                carreraComboBox.ValueMember = "Id";

                if (alumnoId.HasValue)
                {
                    Text = "Editar Alumno";
                    var alumno = await AlumnoApiClient.GetAsync(alumnoId.Value);
                    CargarDatos(alumno);
                }
                else
                {
                    Text = "Nuevo Alumno";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void CargarDatos(AlumnoDTO alumno)
        {
            nomAlumnoTextBox.Text = alumno.NomAlumno;
            apeAlumnoTextBox.Text = alumno.ApeAlumno;
            emailTextBox.Text = alumno.Email;
            legajoTextBox.Text = alumno.Legajo;
            dniTextBox.Text = alumno.Dni;
            planTextBox.Text = alumno.Plan;
            anioCursoNumeric.Value = alumno.AnioCurso;
            cantMatApNumeric.Value = alumno.CantMatAp;
            promedioNumeric.Value = (decimal)alumno.Promedio;
            carreraComboBox.SelectedValue = alumno.CarreraId;
        }

        private async void guardarButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            var dto = new AlumnoDTO
            {
                Id = alumnoId ?? 0,
                NomAlumno = nomAlumnoTextBox.Text.Trim(),
                ApeAlumno = apeAlumnoTextBox.Text.Trim(),
                Email = emailTextBox.Text.Trim(),
                Legajo = legajoTextBox.Text.Trim(),
                Dni = dniTextBox.Text.Trim(),
                Plan = planTextBox.Text.Trim(),
                AnioCurso = (int)anioCursoNumeric.Value,
                CantMatAp = (int)cantMatApNumeric.Value,
                Promedio = (float)promedioNumeric.Value,
                CarreraId = (int)(carreraComboBox.SelectedValue ?? 0)
            };

            try
            {
                Cursor = Cursors.WaitCursor;
                guardarButton.Enabled = false;

                if (alumnoId.HasValue)
                    await AlumnoApiClient.UpdateAsync(dto);
                else
                    await AlumnoApiClient.AddAsync(dto);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                guardarButton.Enabled = true;
            }
        }

        private void cancelarButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateInput()
        {
            errorProvider.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(nomAlumnoTextBox.Text))
            {
                errorProvider.SetError(nomAlumnoTextBox, "El nombre es requerido");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(apeAlumnoTextBox.Text))
            {
                errorProvider.SetError(apeAlumnoTextBox, "El apellido es requerido");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(emailTextBox.Text) || !emailTextBox.Text.Contains('@'))
            {
                errorProvider.SetError(emailTextBox, "Ingrese un email válido");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(legajoTextBox.Text))
            {
                errorProvider.SetError(legajoTextBox, "El legajo es requerido");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(dniTextBox.Text))
            {
                errorProvider.SetError(dniTextBox, "El DNI es requerido");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(planTextBox.Text))
            {
                errorProvider.SetError(planTextBox, "El plan es requerido");
                isValid = false;
            }

            if (carreraComboBox.SelectedValue == null)
            {
                errorProvider.SetError(carreraComboBox, "Seleccione una carrera");
                isValid = false;
            }

            return isValid;
        }
    }
}
