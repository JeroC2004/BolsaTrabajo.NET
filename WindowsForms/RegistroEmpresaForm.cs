using API.Clients;
using DTOs;

namespace WindowsForms
{
    internal sealed class RegistroEmpresaForm : DetalleCatalogoForm
    {
        private readonly TextBox razon, rubro, descripcion, email, password, confirmacion;
        public string UsernameRegistrado => razon.Text.Trim();

        public RegistroEmpresaForm() : base("Registro de empresa — solicitud de aprobación")
        {
            razon = AgregarCampoTexto("Razón social (usuario de acceso)", 150);
            rubro = AgregarCampoTexto("Rubro", 100);
            descripcion = AgregarCampoTexto("Descripción", 500, multilinea: true);
            email = AgregarCampoTexto("Email de contacto", 150);
            password = AgregarCampoTexto("Contraseña (mín. 6)", 256);
            confirmacion = AgregarCampoTexto("Confirmar contraseña", 256);
            password.UseSystemPasswordChar = confirmacion.UseSystemPasswordChar = true;
        }

        protected override async Task GuardarAsync()
        {
            var request = new EmpresaRegistroRequest
            {
                RazonSocial = razon.Text.Trim(), Rubro = rubro.Text.Trim(), Descripcion = descripcion.Text.Trim(),
                Email = email.Text.Trim(), Password = password.Text, ConfirmPassword = confirmacion.Text
            };
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(request,
                new System.ComponentModel.DataAnnotations.ValidationContext(request), true);
            await new AuthApiClient(ApiHttpClientFactory.Shared).RegisterEmpresaAsync(request);
            MessageBox.Show(this, "Solicitud enviada. Podrá iniciar sesión cuando el administrador apruebe el registro.", "Registro de empresa");
        }
    }
}
