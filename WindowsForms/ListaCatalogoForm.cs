namespace WindowsForms
{
    
    internal sealed class ListaCatalogoForm<TDto> : Form where TDto : class
    {
        private readonly DataGridView grilla = new();
        private readonly string nombreEntidad;
        private readonly Func<Task<IEnumerable<TDto>>> cargar;
        private readonly Func<TDto, int> obtenerId;
        private readonly Func<TDto?, Form> crearDetalle;
        private readonly Func<int, Task> eliminar;
        private readonly bool permiteModificar;

        public ListaCatalogoForm(
            string titulo,
            string nombreEntidad,
            IEnumerable<(string Propiedad, string Encabezado, int Ancho)> columnas,
            Func<Task<IEnumerable<TDto>>> cargar,
            Func<TDto, int> obtenerId,
            Func<TDto?, Form> crearDetalle,
            Func<int, Task> eliminar,
            bool permiteAgregar = true,
            bool permiteModificar = true,
            bool permiteEliminar = true)
        {
            this.permiteModificar = permiteModificar;
            this.nombreEntidad = nombreEntidad;
            this.cargar = cargar;
            this.obtenerId = obtenerId;
            this.crearDetalle = crearDetalle;
            this.eliminar = eliminar;

            Text = titulo;
            ClientSize = new Size(900, 480);

            ConfigurarGrilla(columnas);

            var panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 48,
                Padding = new Padding(8, 8, 8, 0)
            };
            if (permiteAgregar)
                panelBotones.Controls.Add(CrearBoton("Agregar", agregarButton_Click));
            if (permiteModificar)
                panelBotones.Controls.Add(CrearBoton("Modificar", modificarButton_Click));
            if (permiteEliminar)
                panelBotones.Controls.Add(CrearBoton("Eliminar", eliminarButton_Click));
            panelBotones.Controls.Add(CrearBoton("Actualizar", actualizarButton_Click));

            // Primero la grilla (Fill) y después el panel (Top) para que el panel quede arriba.
            Controls.Add(grilla);
            Controls.Add(panelBotones);

            Load += async (s, e) => await CargarAsync();
        }

        private void ConfigurarGrilla(IEnumerable<(string Propiedad, string Encabezado, int Ancho)> columnas)
        {
            grilla.Dock = DockStyle.Fill;
            grilla.AutoGenerateColumns = false;
            grilla.ReadOnly = true;
            grilla.AllowUserToAddRows = false;
            grilla.AllowUserToDeleteRows = false;
            grilla.MultiSelect = false;
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

            grilla.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && permiteModificar)
                    modificarButton_Click(s, e);
            };

            // Las fechas se muestran como dd/MM/yyyy.
            grilla.CellFormatting += (s, e) =>
            {
                if (e.Value is DateTime fecha)
                {
                    e.Value = fecha.ToString("dd/MM/yyyy");
                    e.FormattingApplied = true;
                }
            };
        }

        private static Button CrearBoton(string texto, EventHandler onClick)
        {
            var boton = new Button { Text = texto, AutoSize = true };
            boton.Click += onClick;
            return boton;
        }

        private async Task CargarAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var resultado = await cargar();
                grilla.DataSource = null;
                grilla.DataSource = resultado.ToList();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión expiró. Vuelva a iniciar sesión.", "Sesión expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private TDto? ObtenerSeleccionado() => grilla.CurrentRow?.DataBoundItem as TDto;

        private async void agregarButton_Click(object? sender, EventArgs e)
        {
            using Form detalle = crearDetalle(null);
            if (detalle.ShowDialog(this) == DialogResult.OK)
                await CargarAsync();
        }

        private async void modificarButton_Click(object? sender, EventArgs e)
        {
            TDto? seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show($"Seleccione un registro de la lista ({nombreEntidad}).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using Form detalle = crearDetalle(seleccionado);
            if (detalle.ShowDialog(this) == DialogResult.OK)
                await CargarAsync();
        }

        private async void eliminarButton_Click(object? sender, EventArgs e)
        {
            TDto? seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show($"Seleccione un registro de la lista ({nombreEntidad}).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmar = MessageBox.Show($"¿Está seguro que desea eliminar: {nombreEntidad} seleccionado?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                await eliminar(obtenerId(seleccionado));
                await CargarAsync();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión expiró. Vuelva a iniciar sesión.", "Sesión expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void actualizarButton_Click(object? sender, EventArgs e) => await CargarAsync();
    }
}
