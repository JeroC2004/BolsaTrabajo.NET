// Guía de estudio: Editor de TipoOferta: carga los datos necesarios, permite modificarlos y solicita su guardado mediante el cliente de la API.
using API.Clients;
using DTOs;

namespace WindowsForms
{
    internal sealed partial class TipoOfertaDetalleForm : DetalleCatalogoForm
    {
        private readonly TipoOfertaApiClient client = new(ApiHttpClientFactory.Shared);
        private readonly TipoOfertaDTO tipoOferta;
        private readonly TextBox nombreTextBox;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public TipoOfertaDetalleForm(TipoOfertaDTO? existente)
            : base(existente == null ? "Nuevo tipo de oferta" : "Modificar tipo de oferta")
        {
            tipoOferta = existente == null
                ? new TipoOfertaDTO()
                : new TipoOfertaDTO { Id = existente.Id, Nombre = existente.Nombre };

            nombreTextBox = AgregarCampoTexto("Nombre", 100);
            nombreTextBox.Text = tipoOferta.Nombre;
        }

        // Reúne los datos editados y ejecuta el guardado, mostrando el resultado o los errores de la operación.
        protected override async Task GuardarAsync()
        {
            tipoOferta.Nombre = nombreTextBox.Text.Trim();

            System.ComponentModel.DataAnnotations.Validator.ValidateObject(
                tipoOferta, new System.ComponentModel.DataAnnotations.ValidationContext(tipoOferta), validateAllProperties: true);

            if (tipoOferta.Id == 0)
                await client.AddAsync(tipoOferta);
            else
                await client.UpdateAsync(tipoOferta);
        }
    }
}
