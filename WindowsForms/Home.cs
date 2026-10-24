using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class Home : Form
    {
        private AlumnoLista? alumnoListaForm;
        private OfertaLista? ofertaListaForm;
        private Form? empresaListaForm;
        private Form? carreraListaForm;
        private Form? tipoOfertaListaForm;
        private Form? postulacionListaForm;
        private ToolStripMenuItem? postulacionesMenuItem;
        private ToolStripMenuItem? administracionMenuItem;

        public Home()
        {
            InitializeComponent();
            CrearMenuAdministracion();
            CrearMenuPostulaciones();
        }

        private async void Home_Load(object sender, EventArgs e)
        {
            await Sesion.CargarAsync();

            var authService = AuthServiceProvider.Instance;
            var username = await authService.GetUsernameAsync();
            usuarioConectadoLabel.Text = $"Conectado como: {username} ({Sesion.Rol})";

            // Solo Administrador y Empresa pueden consultar el listado de alumnos.
            alumnosToolStripMenuItem.Visible = Sesion.EsAdministrador || Sesion.EsEmpresa;

            // Los catálogos (empresas, carreras y tipos de oferta) los administra solo el administrador.
            if (administracionMenuItem != null)
                administracionMenuItem.Visible = Sesion.EsAdministrador;

            // El alumno ve sus propias postulaciones; la empresa, las de sus ofertas; el administrador, todas.
            if (postulacionesMenuItem != null)
                postulacionesMenuItem.Text = Sesion.EsAlumno ? "Mis postulaciones" : "Postulaciones";
        }

        private void alumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirVentanaHija(alumnoListaForm, f => alumnoListaForm = f, () => new AlumnoLista());
        }

        private void ofertasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirVentanaHija(ofertaListaForm, f => ofertaListaForm = f, () => new OfertaLista());
        }


        private void CrearMenuAdministracion()
        {
            administracionMenuItem = new ToolStripMenuItem("Administración") { Visible = false };

            administracionMenuItem.DropDownItems.Add("Empresas", null,
                (s, e) => AbrirVentanaHija<Form>(empresaListaForm, f => empresaListaForm = f, CrearListaEmpresas));
            administracionMenuItem.DropDownItems.Add("Carreras", null,
                (s, e) => AbrirVentanaHija<Form>(carreraListaForm, f => carreraListaForm = f, CrearListaCarreras));
            administracionMenuItem.DropDownItems.Add("Tipos de oferta", null,
                (s, e) => AbrirVentanaHija<Form>(tipoOfertaListaForm, f => tipoOfertaListaForm = f, CrearListaTiposOferta));

            int posicion = menuStrip1.Items.IndexOf(ofertasToolStripMenuItem) + 1;
            menuStrip1.Items.Insert(posicion, administracionMenuItem);
        }

        private void CrearMenuPostulaciones()
        {
            postulacionesMenuItem = new ToolStripMenuItem("Postulaciones");
            postulacionesMenuItem.Click += (s, e) =>
                AbrirVentanaHija<Form>(postulacionListaForm, f => postulacionListaForm = f, CrearListaPostulaciones);

            int posicion = menuStrip1.Items.IndexOf(ofertasToolStripMenuItem) + 1;
            menuStrip1.Items.Insert(posicion, postulacionesMenuItem);
        }

        private static Form CrearListaPostulaciones()
        {
            var client = new PostulacionApiClient(ApiHttpClientFactory.Shared);

            return new ListaCatalogoForm<PostulacionDTO>(
                Sesion.EsAlumno ? "Mis postulaciones" : "Postulaciones",
                "la postulación",
                new[]
                {
                    ("FechaPostulacion", "Fecha", 90),
                    ("AlumnoNombre", "Alumno", 190),
                    ("AlumnoLegajo", "Legajo", 70),
                    ("AlumnoCarrera", "Carrera", 200),
                    ("AlumnoPromedio", "Prom.", 60),
                    ("OfertaTitulo", "Oferta", 220),
                    ("EmpresaNombre", "Empresa", 160),
                    ("Estado", "Estado", 90)
                },
                () => client.GetAllAsync(),
                postulacion => postulacion.Id,
                postulacion => new PostulacionDetalleForm(postulacion),
                id => client.DeleteAsync(id),
                permiteAgregar: Sesion.EsAdministrador,
                permiteModificar: Sesion.EsAdministrador || Sesion.EsEmpresa,
                permiteEliminar: Sesion.EsAdministrador || Sesion.EsAlumno);
        }

        private static Form CrearListaEmpresas()
        {
            var client = new EmpresaApiClient(ApiHttpClientFactory.Shared);

            return new ListaCatalogoForm<EmpresaDTO>(
                "Empresas",
                "la empresa",
                new[] { ("RazonSocial", "Razón social", 230), ("Rubro", "Rubro", 150), ("Descripcion", "Descripción", 400) },
                () => client.GetAllAsync(),
                empresa => empresa.Id,
                empresa => new EmpresaDetalleForm(empresa),
                id => client.DeleteAsync(id));
        }

        private static Form CrearListaCarreras()
        {
            var client = new CarreraApiClient(ApiHttpClientFactory.Shared);

            return new ListaCatalogoForm<CarreraDTO>(
                "Carreras",
                "la carrera",
                new[] { ("NomCarrera", "Carrera", 350), ("Departamento", "Departamento", 200), ("Duracion", "Duración (años)", 120) },
                () => client.GetAllAsync(),
                carrera => carrera.Id,
                carrera => new CarreraDetalleForm(carrera),
                id => client.DeleteAsync(id));
        }

        private static Form CrearListaTiposOferta()
        {
            var client = new TipoOfertaApiClient(ApiHttpClientFactory.Shared);

            return new ListaCatalogoForm<TipoOfertaDTO>(
                "Tipos de oferta",
                "el tipo de oferta",
                new[] { ("Nombre", "Nombre", 400) },
                () => client.GetAllAsync(),
                tipo => tipo.Id,
                tipo => new TipoOfertaDetalleForm(tipo),
                id => client.DeleteAsync(id));
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
