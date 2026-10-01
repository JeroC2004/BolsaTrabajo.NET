using API.Clients;

namespace WindowsForms
{
    public partial class Home : Form
    {
        private AlumnoLista? alumnoListaForm;
        private OfertaLista? ofertaListaForm;

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
            AbrirVentanaHija(alumnoListaForm, f => alumnoListaForm = f, () => new AlumnoLista());
        }

        private void ofertasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirVentanaHija(ofertaListaForm, f => ofertaListaForm = f, () => new OfertaLista());
        }

        
        private void AbrirVentanaHija<TForm>(TForm? instanciaActual, Action<TForm?> asignarInstancia, Func<TForm> crearForm) where TForm : Form
        {
            if (instanciaActual == null || instanciaActual.IsDisposed)
            {
                var nuevaForm = crearForm();
                nuevaForm.MdiParent = this;
                nuevaForm.FormClosed += (s, args) => asignarInstancia(null);
                nuevaForm.Show();
                asignarInstancia(nuevaForm);
            }
            else
            {
                instanciaActual.Activate();
            }
        }

        private void cascadaToolStripMenuItem_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.Cascade);

        private void mosaicoToolStripMenuItem_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileHorizontal);

        private async void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar la sesión?", "Cerrar sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                foreach (var child in MdiChildren)
                    child.Close();

                var authService = AuthServiceProvider.Instance;
                await authService.LogoutAsync();

                Close();
            }
        }
    }
}
