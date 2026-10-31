// Guía de estudio: Editor de ofertas: reúne datos de empresa, tipo, fechas y requisitos y envía el DTO a la API para crear o modificar la oferta.
using API.Clients;
using Domain.Model;
using DTOs;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class OfertaDetalle : Form
    {
        private readonly int? ofertaId;
        private List<EmpresaDTO> empresas = new();
        private List<TipoOfertaDTO> tiposOferta = new();

        private readonly OfertaApiClient _ofertaClient;
        private readonly EmpresaApiClient _empresaClient;
        private readonly TipoOfertaApiClient _tipoOfertaClient;

        private BindingList<RequisitoOfertaDTO> listaRequisitos = new();

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public OfertaDetalle()
        {
            InitializeComponent();
            ofertaId = null;

            var httpClient = ApiHttpClientFactory.Shared;
            _ofertaClient = new OfertaApiClient(httpClient);
            _empresaClient = new EmpresaApiClient(httpClient);
            _tipoOfertaClient = new TipoOfertaApiClient(httpClient);

            ConfigurarGrilla();
        }

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public OfertaDetalle(int id)
        {
            InitializeComponent();
            ofertaId = id;

            var httpClient = ApiHttpClientFactory.Shared;
            _ofertaClient = new OfertaApiClient(httpClient);
            _empresaClient = new EmpresaApiClient(httpClient);
            _tipoOfertaClient = new TipoOfertaApiClient(httpClient);

            ConfigurarGrilla();
        }

        // Prepara controles y propiedades visuales para presentar los datos y conectar las acciones de la pantalla.
        private void ConfigurarGrilla()
        {
            requisitosGridView.AutoGenerateColumns = false;
            requisitosGridView.Columns.Clear();
            requisitosGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descripcion",
                HeaderText = "Descripción",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            requisitosGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "EsExcluyente",
                HeaderText = "Excluyente",
                Width = 75
            });

            requisitosGridView.ReadOnly = true;
            requisitosGridView.AllowUserToAddRows = false;
            requisitosGridView.AllowUserToDeleteRows = false;
            requisitosGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            requisitosGridView.RowHeadersVisible = false;
            requisitosGridView.DataSource = listaRequisitos;
        }

        // Carga los datos necesarios y actualiza los controles de la pantalla.
        private async void OfertaDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var resultadoEmpresas = await _empresaClient.GetAllAsync();
                empresas = resultadoEmpresas.ToList();

                if (Sesion.EsEmpresa && Sesion.EmpresaId.HasValue)
                {
                    empresas = empresas.Where(emp => emp.Id == Sesion.EmpresaId.Value).ToList();
                    empresaComboBox.Enabled = false;
                }

                empresaComboBox.DataSource = empresas;
                empresaComboBox.DisplayMember = "RazonSocial";
                empresaComboBox.ValueMember = "Id";

                var resultadoTipos = await _tipoOfertaClient.GetAllAsync();
                tiposOferta = resultadoTipos.ToList();
                tipoOfertaComboBox.DataSource = tiposOferta;
                tipoOfertaComboBox.DisplayMember = "Nombre";
                tipoOfertaComboBox.ValueMember = "Id";

                tipoVinculoComboBox.DataSource = Enum.GetNames(typeof(TipoVinculo));
                estadoComboBox.DataSource = Enum.GetNames(typeof(EstadoOferta));

                if (ofertaId.HasValue)
                {
                    Text = "Editar Oferta";
                    var oferta = await _ofertaClient.GetAsync(ofertaId.Value);
                    CargarDatos(oferta);
                }
                else
                {
                    Text = "Nueva Oferta";
                    fechaDesdePicker.Value = DateTime.Today;
                    fechaHastaPicker.Value = DateTime.Today.AddMonths(3);
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
        private void CargarDatos(OfertaDTO oferta)
        {
            tituloTextBox.Text = oferta.Titulo;
            empresaComboBox.SelectedValue = oferta.EmpresaId;
            tipoOfertaComboBox.SelectedValue = oferta.TipoOfertaId;
            tipoVinculoComboBox.SelectedItem = oferta.TipoVinculo;
            estadoComboBox.SelectedItem = oferta.Estado;
            fechaDesdePicker.Value = oferta.FechaDesde;
            fechaHastaPicker.Value = oferta.FechaHasta;
            detalleTextBox.Text = oferta.Detalle;

            listaRequisitos.Clear();
            if (oferta.Requisitos != null)
            {
                foreach (var req in oferta.Requisitos)
                {
                    listaRequisitos.Add(req);
                }
            }
        }

        // Procesa los datos ingresados y muestra el resultado de la operación solicitada.
        private async void guardarButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            var dto = new OfertaDTO
            {
                Id = ofertaId ?? 0,
                Titulo = tituloTextBox.Text.Trim(),
                EmpresaId = (int)(empresaComboBox.SelectedValue ?? 0),
                TipoOfertaId = (int)(tipoOfertaComboBox.SelectedValue ?? 0),
                TipoVinculo = tipoVinculoComboBox.SelectedItem!.ToString()!,
                Estado = estadoComboBox.SelectedItem!.ToString()!,
                FechaDesde = fechaDesdePicker.Value.Date,
                FechaHasta = fechaHastaPicker.Value.Date,
                Detalle = detalleTextBox.Text.Trim(),

                Requisitos = listaRequisitos.ToList()
            };

            try
            {
                Cursor = Cursors.WaitCursor;
                guardarButton.Enabled = false;

                if (ofertaId.HasValue)
                    await _ofertaClient.UpdateAsync(dto);
                else
                    await _ofertaClient.AddAsync(dto);

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

            if (string.IsNullOrWhiteSpace(tituloTextBox.Text))
            {
                errorProvider.SetError(tituloTextBox, "El título es requerido");
                isValid = false;
            }

            if (empresaComboBox.SelectedValue == null)
            {
                errorProvider.SetError(empresaComboBox, "Seleccione una empresa");
                isValid = false;
            }

            if (tipoOfertaComboBox.SelectedValue == null)
            {
                errorProvider.SetError(tipoOfertaComboBox, "Seleccione un tipo de oferta");
                isValid = false;
            }

            if (fechaHastaPicker.Value.Date < fechaDesdePicker.Value.Date)
            {
                errorProvider.SetError(fechaHastaPicker, "La fecha hasta no puede ser anterior a la fecha desde");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(detalleTextBox.Text))
            {
                errorProvider.SetError(detalleTextBox, "El detalle es requerido");
                isValid = false;
            }

            if (listaRequisitos.Count == 0)
            {
                errorProvider.SetError(requisitosGridView, "Debe agregar al menos un requisito a la oferta");
                isValid = false;
            }

            return isValid;
        }

        // Manejador de evento sin implementación: actualmente no realiza ninguna acción.
        private void tituloTextBox_TextChanged(object sender, EventArgs e) { }
        // Manejador de evento sin implementación: actualmente no realiza ninguna acción.
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        // Manejador de evento sin implementación: actualmente no realiza ninguna acción.
        private void label1_Click(object sender, EventArgs e) { }
        // Manejador de evento sin implementación: actualmente no realiza ninguna acción.
        private void EsExcluyentechk_CheckedChanged(object sender, EventArgs e) { }

        // Abre o navega a la pantalla correspondiente a la acción seleccionada.
        private void agregarButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(requisitoTextBox.Text))
            {
                MessageBox.Show("Ingrese una descripción para el requisito.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            listaRequisitos.Add(new RequisitoOfertaDTO
            {
                Descripcion = requisitoTextBox.Text.Trim(),
                EsExcluyente = EsExcluyentechk.Checked
            });

            requisitoTextBox.Clear();
            EsExcluyentechk.Checked = false;
            requisitoTextBox.Focus();
        }

        // Quita de la colección editable el requisito seleccionado antes de guardar la oferta.
        private void btnEliminarRequisito_Click_1(object sender, EventArgs e)
        {
            if (requisitosGridView.CurrentRow != null)
            {
                var requisitoSeleccionado = (RequisitoOfertaDTO)requisitosGridView.CurrentRow.DataBoundItem;
                listaRequisitos.Remove(requisitoSeleccionado);
            }
        }
    }
}