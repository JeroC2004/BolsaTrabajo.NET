using API.Clients;
using DTOs;

namespace WindowsForms
{
    internal sealed class CurriculumForm : Form
    {
        private const int TamanioMaximoBytes = 5 * 1024 * 1024;

        private readonly CurriculumApiClient client = new(ApiHttpClientFactory.Shared);
        private readonly int alumnoId;

        private readonly Label estadoLabel = new();
        private readonly Button cargarButton = new();
        private readonly Button descargarButton = new();
        private readonly Button eliminarButton = new();

        public CurriculumForm(int alumnoId)
        {
            this.alumnoId = alumnoId;

            Text = "Mi currículum";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(460, 190);

            estadoLabel.SetBounds(16, 16, 428, 80);
            estadoLabel.Text = "Cargando...";

            var ayuda = new Label
            {
                Text = "Solo se admiten archivos PDF de hasta 5 MB. Las empresas a las que te postules podrán descargarlo.",
                ForeColor = SystemColors.GrayText
            };
            ayuda.SetBounds(16, 100, 428, 36);

            cargarButton.Text = "Cargar / reemplazar...";
            cargarButton.SetBounds(16, 146, 150, 30);
            cargarButton.Click += async (s, e) => await CargarArchivoAsync();

            descargarButton.Text = "Descargar";
            descargarButton.SetBounds(176, 146, 120, 30);
            descargarButton.Click += async (s, e) => await EjecutarAsync(
                () => CurriculumDescarga.DescargarAsync(this, client, alumnoId));

            eliminarButton.Text = "Eliminar";
            eliminarButton.SetBounds(306, 146, 120, 30);
            eliminarButton.Click += async (s, e) => await EliminarAsync();

            Controls.AddRange(new Control[] { estadoLabel, ayuda, cargarButton, descargarButton, eliminarButton });

            Shown += async (s, e) => await ActualizarAsync();
        }

        private async Task ActualizarAsync()
        {
            await EjecutarAsync(async () =>
            {
                CurriculumDTO? info = await client.GetInfoAsync(alumnoId);

                if (info == null)
                {
                    estadoLabel.Text = "Todavía no cargaste tu currículum.";
                }
                else
                {
                    estadoLabel.Text = $"Archivo: {info.NombreArchivo}\n" +
                                       $"Tamaño: {info.Tamanio / 1024.0:0.#} KB\n" +
                                       $"Cargado el: {info.FechaCarga:dd/MM/yyyy HH:mm}";
                }

                descargarButton.Enabled = info != null;
                eliminarButton.Enabled = info != null;
            });
        }

        private async Task CargarArchivoAsync()
        {
            using var dialogo = new OpenFileDialog
            {
                Title = "Seleccionar currículum",
                Filter = "Documento PDF (*.pdf)|*.pdf",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialogo.ShowDialog(this) != DialogResult.OK)
                return;

            var archivo = new FileInfo(dialogo.FileName);

            if (!archivo.Extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("El currículum debe ser un archivo PDF.", "Archivo no válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (archivo.Length == 0 || archivo.Length > TamanioMaximoBytes)
            {
                MessageBox.Show("El archivo debe pesar entre 1 byte y 5 MB.", "Archivo no válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await EjecutarAsync(async () =>
            {
                byte[] contenido = await File.ReadAllBytesAsync(archivo.FullName);
                await client.SubirAsync(alumnoId, archivo.Name, contenido);
                await ActualizarAsync();
                MessageBox.Show("Currículum cargado correctamente.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        private async Task EliminarAsync()
        {
            var confirmar = MessageBox.Show("¿Está seguro que desea eliminar su currículum?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes)
                return;

            await EjecutarAsync(async () =>
            {
                await client.EliminarAsync(alumnoId);
                await ActualizarAsync();
            });
        }

        private async Task EjecutarAsync(Func<Task> accion)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                await accion();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión expiró. Vuelva a iniciar sesión.", "Sesión expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo completar la operación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
