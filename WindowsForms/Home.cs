using API.Clients;

namespace WindowsForms
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private async void Home_Load(object sender, EventArgs e)
        {
            var authService = AuthServiceProvider.Instance;
            var username = await authService.GetUsernameAsync();
            usuarioConectadoLabel.Text = $"Conectado como: {username}";
        }

        private void alumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlumnoLista alumnosForm = new AlumnoLista();
            alumnosForm.ShowDialog();
        }

        private void ofertasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OfertaLista ofertasForm = new OfertaLista();
            ofertasForm.ShowDialog();
        }

        private async void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar la sesión?", "Cerrar sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var authService = AuthServiceProvider.Instance;
                await authService.LogoutAsync();

                // Cierra la pantalla principal; Program.cs vuelve a mostrar el LoginForm
                this.Close();
            }
        }
    }
}
