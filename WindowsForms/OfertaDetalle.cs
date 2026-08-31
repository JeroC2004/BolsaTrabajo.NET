using API.Clients;
using Domain.Model;
using DTOs;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class OfertaDetalle : Form
    {
        private readonly int? ofertaId;
        private List<EmpresaDTO> empresas = new();
        private List<TipoOfertaDTO> tiposOferta = new();

        // Constructor sin parámetros: modo alta
        public OfertaDetalle()
        {
            InitializeComponent();
            ofertaId = null;
        }

        // Constructor con id: modo edición
        public OfertaDetalle(int id)
        {
            InitializeComponent();
            ofertaId = id;
        }

        private async void OfertaDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Combos de lookup (vienen de la API)
                var resultadoEmpresas = await EmpresaApiClient.GetAllAsync();
                empresas = resultadoEmpresas.ToList();
                empresaComboBox.DataSource = empresas;
                empresaComboBox.DisplayMember = "RazonSocial";
                empresaComboBox.ValueMember = "Id";

                var resultadoTipos = await TipoOfertaApiClient.GetAllAsync();
                tiposOferta = resultadoTipos.ToList();
                tipoOfertaComboBox.DataSource = tiposOferta;
                tipoOfertaComboBox.DisplayMember = "Nombre";
                tipoOfertaComboBox.ValueMember = "Id";

                // Combos de enum de dominio (no dependen de la API, se listan directo del enum)
                tipoVinculoComboBox.DataSource = Enum.GetNames(typeof(TipoVinculo));
                estadoComboBox.DataSource = Enum.GetNames(typeof(EstadoOferta));

                if (ofertaId.HasValue)
                {
                    Text = "Editar Oferta";
                    var oferta = await OfertaApiClient.GetAsync(ofertaId.Value);
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
            requisitosTextBox.Text = oferta.Requisitos;
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
                Requisitos = requisitosTextBox.Text.Trim()
            };

            try
            {
                Cursor = Cursors.WaitCursor;
                guardarButton.Enabled = false;

                if (ofertaId.HasValue)
                    await OfertaApiClient.UpdateAsync(dto);
                else
                    await OfertaApiClient.AddAsync(dto);

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

            if (string.IsNullOrWhiteSpace(requisitosTextBox.Text))
            {
                errorProvider.SetError(requisitosTextBox, "Los requisitos son requeridos");
                isValid = false;
            }

            return isValid;
        }
    }
}
