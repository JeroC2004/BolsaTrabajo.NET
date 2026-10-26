using API.Clients;

namespace WindowsForms
{
    /// <summary>Descarga el CV de un alumno y lo guarda donde el usuario elija.</summary>
    internal static class CurriculumDescarga
    {
        public static async Task DescargarAsync(IWin32Window? propietario, CurriculumApiClient client, int alumnoId, string? nombreSugerido = null)
        {
            var archivo = await client.DescargarAsync(alumnoId);

            using var dialogo = new SaveFileDialog
            {
                Title = "Guardar currículum",
                Filter = "Documento PDF (*.pdf)|*.pdf",
                FileName = string.IsNullOrWhiteSpace(nombreSugerido) ? archivo.NombreArchivo : nombreSugerido,
                OverwritePrompt = true
            };

            if (dialogo.ShowDialog(propietario) != DialogResult.OK)
                return;

            await File.WriteAllBytesAsync(dialogo.FileName, archivo.Contenido);

            var abrir = MessageBox.Show($"CV guardado en:\n{dialogo.FileName}\n\n¿Desea abrirlo ahora?", "Currículum descargado",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (abrir == DialogResult.Yes)
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(dialogo.FileName) { UseShellExecute = true });
        }
    }
}
