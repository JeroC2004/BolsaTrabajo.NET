using API.Clients;
using DTOs;

namespace WindowsForms
{
    internal sealed partial class TipoOfertaDetalleForm : DetalleCatalogoForm
    {
        private readonly TipoOfertaApiClient client = new(ApiHttpClientFactory.Shared);
        private readonly TipoOfertaDTO tipoOferta;
        private readonly TextBox nombreTextBox;

        public TipoOfertaDetalleForm(TipoOfertaDTO? existente)
            : base(existente == null ? "Nuevo tipo de oferta" : "Modificar tipo de oferta")
        {
            tipoOferta = existente == null
                ? new TipoOfertaDTO()
                : new TipoOfertaDTO { Id = existente.Id, Nombre = existente.Nombre };

            nombreTextBox = AgregarCampoTexto("Nombre", 100);
            nombreTextBox.Text = tipoOferta.Nombre;
        }

        protected override async Task GuardarAsync()
        {
            tipoOferta.Nombre = nombreTextBox.Text.Trim();

            if (tipoOferta.Id == 0)
                await client.AddAsync(tipoOferta);
            else
                await client.UpdateAsync(tipoOferta);
        }
    }
}
