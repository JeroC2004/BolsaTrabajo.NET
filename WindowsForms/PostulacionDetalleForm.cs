using API.Clients;
using DTOs;

namespace WindowsForms
{

    internal sealed partial class PostulacionDetalleForm : DetalleCatalogoForm
    {
        private sealed record Opcion(int Id, string Texto);

        private readonly PostulacionApiClient client = new(ApiHttpClientFactory.Shared);
        private readonly AlumnoApiClient alumnoClient = new(ApiHttpClientFactory.Shared);
        private readonly OfertaApiClient ofertaClient = new(ApiHttpClientFactory.Shared);
        private readonly PostulacionDTO? existente;
        private readonly ComboBox alumnoCombo;
        private readonly ComboBox ofertaCombo;
        private readonly ComboBox estadoCombo;

        public PostulacionDetalleForm(PostulacionDTO? existente)
            : base(existente == null ? "Nueva postulación" : "Modificar postulación")
        {
            this.existente = existente;

            alumnoCombo = AgregarCampoLista("Alumno");
            ofertaCombo = AgregarCampoLista("Oferta");
            estadoCombo = AgregarCampoLista("Estado");

            alumnoCombo.DisplayMember = nameof(Opcion.Texto);
            alumnoCombo.ValueMember = nameof(Opcion.Id);
            ofertaCombo.DisplayMember = nameof(Opcion.Texto);
            ofertaCombo.ValueMember = nameof(Opcion.Id);

            estadoCombo.DataSource = new[] { "Pendiente", "Aceptada", "Rechazada" };

            if (existente == null)
            {
                estadoCombo.Enabled = false;
            }
            else
            {

                alumnoCombo.Enabled = false;
                ofertaCombo.Enabled = false;
                estadoCombo.SelectedItem = existente.Estado;
            }
        }

        protected override async Task OnCargarAsync()
        {
            if (existente == null)
            {
                var alumnos = await alumnoClient.GetAllAsync();
                alumnoCombo.DataSource = alumnos
                    .OrderBy(a => a.ApeAlumno)
                    .Select(a => new Opcion(a.Id, $"{a.ApeAlumno}, {a.NomAlumno} ({a.Legajo})"))
                    .ToList();

                var ofertas = await ofertaClient.GetAllAsync();
                ofertaCombo.DataSource = ofertas
                    .Where(o => o.Estado == "Activa")
                    .Select(o => new Opcion(o.Id, $"{o.Titulo} - {o.EmpresaNombre}"))
                    .ToList();
            }
            else
            {
                alumnoCombo.DataSource = new List<Opcion> { new(existente.AlumnoId, existente.AlumnoNombre ?? existente.AlumnoId.ToString()) };
                ofertaCombo.DataSource = new List<Opcion> { new(existente.OfertaId, existente.OfertaTitulo ?? existente.OfertaId.ToString()) };
            }
        }

        protected override async Task GuardarAsync()
        {
            if (existente == null)
            {
                if (alumnoCombo.SelectedValue is not int alumnoId || ofertaCombo.SelectedValue is not int ofertaId)
                    throw new InvalidOperationException("Seleccione un alumno y una oferta.");

                await client.AddAsync(new PostulacionDTO { AlumnoId = alumnoId, OfertaId = ofertaId });
            }
            else
            {
                string estado = estadoCombo.SelectedItem as string ?? existente.Estado;
                await client.UpdateAsync(new PostulacionDTO { Id = existente.Id, Estado = estado });
            }
        }
    }
}
