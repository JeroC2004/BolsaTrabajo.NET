// Guía de estudio: Pantalla de inicio de sesión de escritorio: captura credenciales y utiliza el servicio de autenticación para acceder al sistema.
using API.Clients;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class LoginForm : Form
    {
        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public LoginForm()
        {
            InitializeComponent();
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + 55);
            var registro = new Button
            {
                Text = "Registrarme como alumno",
                Dock = DockStyle.Bottom,
                Height = 45,
                TabIndex = 7
            };
            registro.Click += (_, _) =>
            {
                using var form = new RegistroForm();
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    usernameTextBox.Text = form.UsernameRegistrado;
                    passwordTextBox.Clear();
                    passwordTextBox.Focus();
                }
            };
            Controls.Add(registro);
        }

        // Envía las credenciales al servicio de autenticación y procesa el resultado del acceso.
        private async void loginButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    loginButton.Enabled = false;
                    loginButton.Text = "Iniciando sesión...";

                    var authService = AuthServiceProvider.Instance;
                    bool success = await authService.LoginAsync(usernameTextBox.Text, passwordTextBox.Text);

                    if (success)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.", "Error de autenticación",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        passwordTextBox.Clear();
                        passwordTextBox.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    loginButton.Enabled = true;
                    loginButton.Text = "Iniciar Sesión";
                }
            }
        }

        // Cierra el formulario sin iniciar sesión.
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Revisa los campos del formulario y señala los errores antes de enviar datos a la API.
        private bool ValidateInput()
        {
            errorProvider.SetError(usernameTextBox, string.Empty);
            errorProvider.SetError(passwordTextBox, string.Empty);

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                errorProvider.SetError(usernameTextBox, "El nombre de usuario es requerido");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                errorProvider.SetError(passwordTextBox, "La contraseña es requerida");
                isValid = false;
            }

            return isValid;
        }

        // Detecta la tecla de confirmación en el campo de contraseña para iniciar el acceso.
        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                loginButton_Click(sender, EventArgs.Empty);
                e.Handled = true;
            }
        }
    }
}
