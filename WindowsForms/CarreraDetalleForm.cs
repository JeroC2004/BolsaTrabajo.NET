using API.Clients;
using DTOs;

namespace WindowsForms
{
    internal sealed partial class CarreraDetalleForm : DetalleCatalogoForm
    {
        private readonly CarreraApiClient client = new(ApiHttpClientFactory.Shared);
        private readonly CarreraDTO carrera;
        private readonly TextBox nombreTextBox;
        private readonly TextBox departamentoTextBox;
        private readonly NumericUpDown duracionNumeric;

        public CarreraDetalleForm(CarreraDTO? existente)
            : base(existente == null ? "Nueva carrera" : "Modificar carrera")
        {
            carrera = existente == null
                ? new CarreraDTO { Duracion = 5 }
                : new CarreraDTO
                {
                    Id = existente.Id,
                    NomCarrera = existente.NomCarrera,
                    Departamento = existente.Departamento,
                    Duracion = existente.Duracion
                };

            nombreTextBox = AgregarCampoTexto("Nombre", 150);
            departamentoTextBox = AgregarCampoTexto("Departamento", 100);
            duracionNumeric = AgregarCampoNumerico("Duración (años)", 1, 10);

            nombreTextBox.Text = carrera.NomCarrera;
            departamentoTextBox.Text = carrera.Departamento;
            duracionNumeric.Value = Math.Clamp(carrera.Duracion, 1, 10);
        }

        protected override async Task GuardarAsync()
        {
            carrera.NomCarrera = nombreTextBox.Text.Trim();
            carrera.Departamento = departamentoTextBox.Text.Trim();
            carrera.Duracion = (int)duracionNumeric.Value;

            System.ComponentModel.DataAnnotations.Validator.ValidateObject(
                carrera, new System.ComponentModel.DataAnnotations.ValidationContext(carrera), validateAllProperties: true);

            if (carrera.Id == 0)
                await client.AddAsync(carrera);
            else
                await client.UpdateAsync(carrera);
        }
    }
}
