using API.Clients;
using DTOs;

namespace WindowsForms
{
    internal sealed partial class EmpresaDetalleForm : DetalleCatalogoForm
    {
        private readonly EmpresaApiClient client = new(ApiHttpClientFactory.Shared);
        private readonly EmpresaDTO empresa;
        private readonly TextBox razonSocialTextBox;
        private readonly TextBox rubroTextBox;
        private readonly TextBox descripcionTextBox;

        public EmpresaDetalleForm(EmpresaDTO? existente)
            : base(existente == null ? "Nueva empresa" : "Modificar empresa")
        {
            empresa = existente == null
                ? new EmpresaDTO()
                : new EmpresaDTO
                {
                    Id = existente.Id,
                    RazonSocial = existente.RazonSocial,
                    Descripcion = existente.Descripcion,
                    Rubro = existente.Rubro
                };

            razonSocialTextBox = AgregarCampoTexto("Razón social", 150);
            rubroTextBox = AgregarCampoTexto("Rubro", 100);
            descripcionTextBox = AgregarCampoTexto("Descripción", 500, multilinea: true);

            razonSocialTextBox.Text = empresa.RazonSocial;
            rubroTextBox.Text = empresa.Rubro;
            descripcionTextBox.Text = empresa.Descripcion;
        }

        protected override async Task GuardarAsync()
        {
            empresa.RazonSocial = razonSocialTextBox.Text.Trim();
            empresa.Rubro = rubroTextBox.Text.Trim();
            empresa.Descripcion = descripcionTextBox.Text.Trim();

            if (empresa.Id == 0)
                await client.AddAsync(empresa);
            else
                await client.UpdateAsync(empresa);
        }
    }
}
