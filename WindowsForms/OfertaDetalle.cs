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

        public OfertaDetalle()
        {
            InitializeComponent();
            ofertaId = null;

            var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5183/") };
            _ofertaClient = new OfertaApiClient(httpClient);
            _empresaClient = new EmpresaApiClient(httpClient);
            _tipoOfertaClient = new TipoOfertaApiClient(httpClient);

            ConfigurarGrilla();
        }

        public OfertaDetalle(int id)
        {
            InitializeComponent();
            ofertaId = id;

            var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5183/") };
            _ofertaClient = new OfertaApiClient(httpClient);
            _empresaClient = new EmpresaApiClient(httpClient);
            _tipoOfertaClient = new TipoOfertaApiClient(httpClient);

            ConfigurarGrilla();
        }

        private void ConfigurarGrilla()
        {
            requisitosGridView.AutoGenerateColumns = true;
            requisitosGridView.DataSource = listaRequisitos;
        }

        private async void OfertaDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var resultadoEmpresas = await _empresaClient.GetAllAsync();
                empresas = resultadoEmpresas.ToList();
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

        private void cancelarButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

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

        private void tituloTextBox_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void EsExcluyentechk_CheckedChanged(object sender, EventArgs e) { }

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