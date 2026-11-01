namespace WindowsForms
{
    internal sealed class RegistroSeleccionForm : Form
    {
        public string UsernameRegistrado { get; private set; } = string.Empty;
        public RegistroSeleccionForm()
        {
            Text = "Registrarse";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(380, 150);
            var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            panel.Controls.Add(new Label { Text = "Seleccione el tipo de cuenta", AutoSize = true, Width = 320 });
            var alumno = new Button { Text = "Alumno", Width = 150, Height = 45 };
            var empresa = new Button { Text = "Empresa", Width = 150, Height = 45 };
            alumno.Click += (_, _) =>
            {
                using var form = new RegistroForm();
                if (form.ShowDialog(this) != DialogResult.OK) return;
                UsernameRegistrado = form.UsernameRegistrado;
                DialogResult = DialogResult.OK;
            };
            empresa.Click += (_, _) =>
            {
                using var form = new RegistroEmpresaForm();
                if (form.ShowDialog(this) != DialogResult.OK) return;
                UsernameRegistrado = form.UsernameRegistrado;
                DialogResult = DialogResult.OK;
            };
            panel.Controls.Add(alumno);
            panel.Controls.Add(empresa);
            Controls.Add(panel);
        }
    }
}
