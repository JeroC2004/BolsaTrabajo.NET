// Guía de estudio: Editor de alumnos: carga carreras y, si recibe un identificador, los datos existentes; valida el formulario y solicita alta o modificación a la API.
using API.Auth.WindowsForms;
using API.Clients;
using DTOs;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class AlumnoDetalle : Form
    {
        private readonly int? alumnoId;
        private List<CarreraDTO> carreras = new();

        private readonly AlumnoApiClient _alumnoClient;
        private readonly CarreraApiClient _carreraClient;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public AlumnoDetalle()
        {
            InitializeComponent();
            alumnoId = null;
            var httpClient = ApiHttpClientFactory.Shared;
            _alumnoClient = new AlumnoApiClient(httpClient);
            _carreraClient = new CarreraApiClient(httpClient);
        }

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public AlumnoDetalle(int id)
        {
            InitializeComponent();
            alumnoId = id;

            var httpClient = ApiHttpClientFactory.Shared;
            _alumnoClient = new AlumnoApiClient(httpClient);
            _carreraClient = new CarreraApiClient(httpClient);
        }

        // Carga los datos necesarios y actualiza los controles de la pantalla.
        private async void AlumnoDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var resultado = await _carreraClient.GetAllAsync();
                carreras = resultado.ToList();
                carreraComboBox.DataSource = carreras;
                carreraComboBox.DisplayMember = "NomCarrera";
                carreraComboBox.ValueMember = "Id";

                if (alumnoId.HasValue)
                {
                    Text = "Editar Alumno";
                    var alumno = await _alumnoClient.GetAsync(alumnoId.Value);
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

        // Copia los valores del DTO a los controles del formulario para permitir su consulta o edición.
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

        // Procesa los datos ingresados y muestra el resultado de la operación solicitada.
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
                    await _alumnoClient.UpdateAsync(dto);
                else
                    await _alumnoClient.AddAsync(dto);

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

        // Sale del editor sin confirmar el guardado de los cambios.
        private void cancelarButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Revisa los campos del formulario y señala los errores antes de enviar datos a la API.
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

            if (!System.Text.RegularExpressions.Regex.IsMatch(legajoTextBox.Text.Trim(), @"^[0-9]+$"))
            {
                errorProvider.SetError(legajoTextBox, "El legajo es obligatorio y debe contener solo números");
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