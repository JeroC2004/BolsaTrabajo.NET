using System.Globalization;
using System.Reflection;
using System.Text;

namespace WindowsForms
{
 
    internal sealed class ReporteForm<TFila> : Form where TFila : class
    {
        private readonly DataGridView grilla = new();
        private readonly GraficoBarras grafico = new();
        private readonly Label resumenLabel = new();
        private readonly DateTimePicker desdePicker = new();
        private readonly DateTimePicker hastaPicker = new();

        private readonly string titulo;
        private readonly string tituloGrafico;
        private readonly List<(string Propiedad, string Encabezado, int Ancho)> columnas;
        private readonly Func<DateTime?, DateTime?, Task<IEnumerable<TFila>>> cargar;
        private readonly Func<IReadOnlyList<TFila>, (List<string> Etiquetas, List<SerieGrafico> Series)> armarGrafico;
        private readonly Func<IReadOnlyList<TFila>, string> resumir;

        private List<TFila> filas = new();

        public ReporteForm(
            string titulo,
            string tituloGrafico,
            IEnumerable<(string Propiedad, string Encabezado, int Ancho)> columnas,
            bool usaFechas,
            Func<DateTime?, DateTime?, Task<IEnumerable<TFila>>> cargar,
            Func<IReadOnlyList<TFila>, (List<string> Etiquetas, List<SerieGrafico> Series)> armarGrafico,
            Func<IReadOnlyList<TFila>, string> resumir)
        {
            this.titulo = titulo;
            this.tituloGrafico = tituloGrafico;
            this.columnas = columnas.ToList();
            this.cargar = cargar;
            this.armarGrafico = armarGrafico;
            this.resumir = resumir;

            Text = titulo;
            ClientSize = new Size(960, 620);

            ConfigurarGrilla();

            resumenLabel.Dock = DockStyle.Fill;
            resumenLabel.TextAlign = ContentAlignment.MiddleLeft;
            resumenLabel.Padding = new Padding(8, 0, 0, 0);
            resumenLabel.Font = new Font(Font, FontStyle.Bold);

            grafico.Dock = DockStyle.Fill;

            var tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 42));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 58));
            tabla.Controls.Add(grilla, 0, 0);
            tabla.Controls.Add(resumenLabel, 0, 1);
            tabla.Controls.Add(grafico, 0, 2);

            var panelFiltros = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(8, 8, 8, 0)
            };

            if (usaFechas)
            {
                ConfigurarSelectorFecha(desdePicker);
                ConfigurarSelectorFecha(hastaPicker);

                panelFiltros.Controls.Add(CrearEtiqueta("Desde:"));
                panelFiltros.Controls.Add(desdePicker);
                panelFiltros.Controls.Add(CrearEtiqueta("Hasta:"));
                panelFiltros.Controls.Add(hastaPicker);
            }

            var generarButton = new Button { Text = "Generar", AutoSize = true };
            generarButton.Click += async (s, e) => await GenerarAsync();

            var exportarButton = new Button { Text = "Exportar a CSV", AutoSize = true };
            exportarButton.Click += (s, e) => ExportarCsv();

            panelFiltros.Controls.Add(generarButton);
            panelFiltros.Controls.Add(exportarButton);

            // Primero el contenido (Fill) y después los filtros (Top) para que queden arriba.
            Controls.Add(tabla);
            Controls.Add(panelFiltros);

            Load += async (s, e) => await GenerarAsync();
        }

        private static Label CrearEtiqueta(string texto) =>
            new() { Text = texto, AutoSize = true, Margin = new Padding(8, 6, 2, 0) };

        // Con la casilla desmarcada no se filtra por esa fecha.
        private static void ConfigurarSelectorFecha(DateTimePicker picker)
        {
            picker.Format = DateTimePickerFormat.Short;
            picker.ShowCheckBox = true;
            picker.Checked = false;
            picker.Width = 115;
        }

        private void ConfigurarGrilla()
        {
            grilla.Dock = DockStyle.Fill;
            grilla.AutoGenerateColumns = false;
            grilla.ReadOnly = true;
            grilla.AllowUserToAddRows = false;
            grilla.AllowUserToDeleteRows = false;
            grilla.RowHeadersVisible = false;
            grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            foreach (var (propiedad, encabezado, ancho) in columnas)
            {
                grilla.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = propiedad,
                    HeaderText = encabezado,
                    Width = ancho
                });
            }
        }

        private async Task GenerarAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                DateTime? desde = desdePicker.ShowCheckBox && desdePicker.Checked ? desdePicker.Value.Date : null;
                DateTime? hasta = hastaPicker.ShowCheckBox && hastaPicker.Checked ? hastaPicker.Value.Date : null;

                var resultado = await cargar(desde, hasta);
                filas = resultado.ToList();

                grilla.DataSource = null;
                grilla.DataSource = filas;

                resumenLabel.Text = resumir(filas);

                var (etiquetas, series) = armarGrafico(filas);
                grafico.SetDatos(tituloGrafico, etiquetas, series);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión expiró. Vuelva a iniciar sesión.", "Sesión expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo generar el reporte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ExportarCsv()
        {
            if (filas.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar. Genere el reporte primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialogo = new SaveFileDialog
            {
                Filter = "Archivo CSV (*.csv)|*.csv",
                FileName = $"{titulo}.csv"
            };

            if (dialogo.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                var csv = new StringBuilder();
                csv.AppendLine(string.Join(";", columnas.Select(c => Escapar(c.Encabezado))));

                foreach (TFila fila in filas)
                {
                    csv.AppendLine(string.Join(";", columnas.Select(c => Escapar(ObtenerValor(fila, c.Propiedad)))));
                }

                // UTF-8 con BOM para que Excel respete los acentos.
                File.WriteAllText(dialogo.FileName, csv.ToString(), new UTF8Encoding(true));

                MessageBox.Show("Reporte exportado correctamente.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string ObtenerValor(TFila fila, string propiedad)
        {
            PropertyInfo? info = typeof(TFila).GetProperty(propiedad);
            object? valor = info?.GetValue(fila);
            return Convert.ToString(valor, CultureInfo.CurrentCulture) ?? string.Empty;
        }

        private static string Escapar(string valor)
        {
            if (valor.IndexOfAny(new[] { ';', '"', '\n', '\r' }) >= 0)
                return "\"" + valor.Replace("\"", "\"\"") + "\"";
            return valor;
        }
    }
}
