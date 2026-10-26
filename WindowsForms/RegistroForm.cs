using API.Clients;
using DTOs;
using System.ComponentModel.DataAnnotations;

namespace WindowsForms
{
    public partial class RegistroForm : Form
    {
        private readonly Dictionary<string, TextBox> campos = new();
        private readonly NumericUpDown anio = new() { Minimum = 1, Maximum = 5, Value = 1 };
        private readonly NumericUpDown materias = new() { Minimum = 0, Maximum = 99 };
        private readonly NumericUpDown promedio = new() { Minimum = 0, Maximum = 10, DecimalPlaces = 2 };
        private readonly ComboBox carrera = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly Button guardar = new() { Text = "Crear cuenta", AutoSize = true, Enabled = false };
        private readonly AuthApiClient client = new(ApiHttpClientFactory.Shared);
        public string UsernameRegistrado { get; private set; } = string.Empty;

        public RegistroForm()
        {
            Text = "Registro de alumno";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(620, 720);
            MinimumSize = new Size(450, 400);
            AutoScaleMode = AutoScaleMode.Font;
            var tabla = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(16) };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            void Fila(string etiqueta, Control control)
            {
                int row = tabla.RowCount++;
                tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tabla.Controls.Add(new Label { Text = etiqueta, AutoSize = true, Margin = new Padding(4, 10, 4, 4) }, 0, row);
                control.Dock = DockStyle.Top;
                control.Margin = new Padding(4, 6, 4, 6);
                tabla.Controls.Add(control, 1, row);
            }
            void Texto(string propiedad, string etiqueta, bool password = false)
            {
                var input = new TextBox { UseSystemPasswordChar = password };
                campos[propiedad] = input;
                Fila(etiqueta, input);
            }
            Texto("Username", "Usuario"); Texto("NomAlumno", "Nombre"); Texto("ApeAlumno", "Apellido");
            Texto("Email", "Email"); Texto("Legajo", "Legajo"); Texto("Dni", "DNI"); Texto("Plan", "Plan");
            Fila("Año de curso", anio); Fila("Materias aprobadas", materias); Fila("Promedio", promedio); Fila("Carrera", carrera);
            Texto("Password", "Contraseña (mín. 6)", true); Texto("ConfirmPassword", "Confirmar contraseña", true);
            var cancelar = new Button { Text = "Cancelar", AutoSize = true, DialogResult = DialogResult.Cancel };
            var acciones = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Bottom, Padding = new Padding(12) };
            acciones.Controls.Add(guardar); acciones.Controls.Add(cancelar);
            var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            scroll.Controls.Add(tabla);
            Controls.Add(scroll); Controls.Add(acciones);
            CancelButton = cancelar;
            guardar.Click += GuardarAsync;
            Shown += async (_, _) =>
            {
                try
                {
                    carrera.DisplayMember = nameof(CarreraDTO.NomCarrera);
                    carrera.ValueMember = nameof(CarreraDTO.Id);
                    carrera.DataSource = (await client.GetCarrerasRegistroAsync()).ToList();
                    carrera.SelectedIndex = -1;
                    guardar.Enabled = true;
                }
                catch (Exception ex) { MessageBox.Show(this, "No se pudieron cargar las carreras: " + ex.Message); }
            };
        }

        private async void GuardarAsync(object? sender, EventArgs e)
        {
            string T(string key) => campos[key].Text.Trim();
            var request = new RegisterRequest
            {
                Username = T("Username"),
                NomAlumno = T("NomAlumno"),
                ApeAlumno = T("ApeAlumno"),
                Email = T("Email"),
                Legajo = T("Legajo"),
                Dni = T("Dni"),
                Plan = T("Plan"),
                AnioCurso = (int)anio.Value,
                CantMatAp = (int)materias.Value,
                Promedio = (float)promedio.Value,
                CarreraId = carrera.SelectedValue is int id ? id : 0,
                Password = campos["Password"].Text,
                ConfirmPassword = campos["ConfirmPassword"].Text
            };
            var errores = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), errores, true))
            {
                MessageBox.Show(this, string.Join(Environment.NewLine, errores.Select(x => x.ErrorMessage)), "Revisá los datos");
                return;
            }
            guardar.Enabled = false;
            try
            {
                var (success, error) = await client.RegisterAsync(request);
                if (!success) { MessageBox.Show(this, error ?? "No se pudo crear la cuenta."); return; }
                UsernameRegistrado = request.Username;
                MessageBox.Show(this, "Cuenta creada. Ya podés iniciar sesión.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, "Error al registrarse"); }
            finally { guardar.Enabled = true; }
        }
    }
}
