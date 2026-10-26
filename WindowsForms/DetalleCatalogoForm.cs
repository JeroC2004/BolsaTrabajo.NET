namespace WindowsForms
{
   
    internal abstract partial class DetalleCatalogoForm : Form
    {
        private readonly TableLayoutPanel tabla = new();
        private readonly Button guardarButton = new();
        private readonly Button cancelarButton = new();
        private int fila;
        private bool botonesAgregados;

        protected DetalleCatalogoForm(string titulo)
        {
            Text = titulo;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(12);

            tabla.ColumnCount = 2;
            tabla.AutoSize = true;
            tabla.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tabla.Dock = DockStyle.Fill;
            Controls.Add(tabla);

            Shown += async (s, e) => await CargarInicialAsync();
        }

        protected virtual Task OnCargarAsync() => Task.CompletedTask;

        private async Task CargarInicialAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                await OnCargarAsync();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión expiró. Vuelva a iniciar sesión.", "Sesión expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        protected TextBox AgregarCampoTexto(string etiqueta, int maxLength, bool multilinea = false)
        {
            var textBox = new TextBox
            {
                Width = 340,
                MaxLength = maxLength,
                Margin = new Padding(3, 3, 3, 8)
            };

            if (multilinea)
            {
                textBox.Multiline = true;
                textBox.Height = 80;
                textBox.AcceptsReturn = true;
                textBox.ScrollBars = ScrollBars.Vertical;
            }

            AgregarFila(etiqueta, textBox);
            return textBox;
        }

        protected ComboBox AgregarCampoLista(string etiqueta)
        {
            var combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 340,
                Margin = new Padding(3, 3, 3, 8)
            };

            AgregarFila(etiqueta, combo);
            return combo;
        }

        protected NumericUpDown AgregarCampoNumerico(string etiqueta, int minimo, int maximo)
        {
            var numerico = new NumericUpDown
            {
                Minimum = minimo,
                Maximum = maximo,
                Width = 120,
                Margin = new Padding(3, 3, 3, 8)
            };

            AgregarFila(etiqueta, numerico);
            return numerico;
        }

        protected abstract Task GuardarAsync();

        private void AgregarFila(string etiqueta, Control control)
        {
            var label = new Label
            {
                Text = etiqueta,
                AutoSize = true,
                Margin = new Padding(3, 6, 12, 8)
            };

            tabla.Controls.Add(label, 0, fila);
            tabla.Controls.Add(control, 1, fila);
            fila++;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!botonesAgregados)
            {
                var panelBotones = new FlowLayoutPanel
                {
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft,
                    Margin = new Padding(3, 8, 3, 3)
                };

                cancelarButton.Text = "Cancelar";
                cancelarButton.AutoSize = true;
                cancelarButton.DialogResult = DialogResult.Cancel;

                guardarButton.Text = "Guardar";
                guardarButton.AutoSize = true;
                guardarButton.Click += guardarButton_Click;

                panelBotones.Controls.Add(cancelarButton);
                panelBotones.Controls.Add(guardarButton);

                tabla.Controls.Add(panelBotones, 0, fila);
                tabla.SetColumnSpan(panelBotones, 2);

                AcceptButton = guardarButton;
                CancelButton = cancelarButton;
                botonesAgregados = true;
            }

            base.OnLoad(e);
        }

        private async void guardarButton_Click(object? sender, EventArgs e)
        {
            try
            {
                guardarButton.Enabled = false;
                Cursor = Cursors.WaitCursor;
                await GuardarAsync();
                DialogResult = DialogResult.OK;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión expiró. Vuelva a iniciar sesión.", "Sesión expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor = Cursors.Default;
                guardarButton.Enabled = true;
            }
        }
    }
}
