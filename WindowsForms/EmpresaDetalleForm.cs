// Guía de estudio: Editor de Empresa: carga los datos necesarios, permite modificarlos y solicita su guardado mediante el cliente de la API.
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

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
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

        // Reúne los datos editados y ejecuta el guardado, mostrando el resultado o los errores de la operación.
        protected override async Task GuardarAsync()
        {
            empresa.RazonSocial = razonSocialTextBox.Text.Trim();
            empresa.Rubro = rubroTextBox.Text.Trim();
            empresa.Descripcion = descripcionTextBox.Text.Trim();

            System.ComponentModel.DataAnnotations.Validator.ValidateObject(
                empresa, new System.ComponentModel.DataAnnotations.ValidationContext(empresa), validateAllProperties: true);

            if (empresa.Id == 0)
                await client.AddAsync(empresa);
            else
                await client.UpdateAsync(empresa);
        }
    }
}
