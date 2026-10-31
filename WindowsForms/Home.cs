// Guía de estudio: Pantalla de inicio y acceso a las funciones del sistema; presenta opciones de navegación según la sesión.
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
        private Form? reportePostulacionesForm;
        private Form? reporteOfertasForm;
        private ToolStripMenuItem? reportesMenuItem;
        private ToolStripMenuItem? postulacionesMenuItem;
        private ToolStripMenuItem? curriculumMenuItem;
        private ToolStripMenuItem? administracionMenuItem;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public Home()
        {
            InitializeComponent();
            CrearMenuAdministracion();
            CrearMenuPostulaciones();
            CrearMenuCurriculum();
            CrearMenuReportes();
        }

        // Carga los datos necesarios y actualiza los controles de la pantalla.
        private async void Home_Load(object sender, EventArgs e)
        {
            var authService = AuthServiceProvider.Instance;
            if (!await authService.IsAuthenticatedAsync())
            {
                using var login = new LoginForm();
                if (login.ShowDialog(this) != DialogResult.OK)
                {
                    Close();
                    return;
                }
            }

            await ActualizarSesionAsync();
        }

        // Abre o navega a la pantalla correspondiente a la acción seleccionada.
        private async Task ActualizarSesionAsync()
        {
            await Sesion.CargarAsync();

            var authService = AuthServiceProvider.Instance;
            var username = await authService.GetUsernameAsync();
            usuarioConectadoLabel.Text = $"Conectado como: {username} ({Sesion.Rol})";

            alumnosToolStripMenuItem.Visible = Sesion.EsAdministrador || Sesion.EsEmpresa;

            if (administracionMenuItem != null)
                administracionMenuItem.Visible = Sesion.EsAdministrador;

            if (postulacionesMenuItem != null)
                postulacionesMenuItem.Text = Sesion.EsAlumno ? "Mis postulaciones" : "Postulaciones";

            if (curriculumMenuItem != null)
                curriculumMenuItem.Visible = Sesion.EsAlumno && Sesion.AlumnoId.HasValue;

            if (reportesMenuItem != null)
                reportesMenuItem.Visible = Sesion.EsAdministrador || Sesion.EsEmpresa;
        }

        // Abre el listado de alumnos desde el menú principal.
        private void alumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirVentanaHija(alumnoListaForm, f => alumnoListaForm = f, () => new AlumnoLista());
        }

        // Abre el listado de ofertas desde el menú principal.
        private void ofertasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirVentanaHija(ofertaListaForm, f => ofertaListaForm = f, () => new OfertaLista());
        }


        // Construye las opciones de administración de catálogos en el menú principal.
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

        // Agrega al menú el acceso a las postulaciones según el rol actual.
        private void CrearMenuPostulaciones()
        {
            postulacionesMenuItem = new ToolStripMenuItem("Postulaciones");
            postulacionesMenuItem.Click += (s, e) =>
                AbrirVentanaHija<Form>(postulacionListaForm, f => postulacionListaForm = f, CrearListaPostulaciones);

            int posicion = menuStrip1.Items.IndexOf(ofertasToolStripMenuItem) + 1;
            menuStrip1.Items.Insert(posicion, postulacionesMenuItem);
        }

        // Agrega el acceso a la gestión del currículum del alumno.
        private void CrearMenuCurriculum()
        {
            curriculumMenuItem = new ToolStripMenuItem("Mi CV") { Visible = false };
            curriculumMenuItem.Click += (s, e) =>
            {
                if (!Sesion.AlumnoId.HasValue)
                    return;

                using var formulario = new CurriculumForm(Sesion.AlumnoId.Value);
                formulario.ShowDialog(this);
            };

            int posicion = menuStrip1.Items.IndexOf(postulacionesMenuItem!) + 1;
            menuStrip1.Items.Insert(posicion, curriculumMenuItem);
        }

        // Agrega al menú los reportes disponibles para el usuario.
        private void CrearMenuReportes()
        {
            reportesMenuItem = new ToolStripMenuItem("Reportes") { Visible = false };

            reportesMenuItem.DropDownItems.Add("Postulaciones por oferta", null,
                (s, e) => AbrirVentanaHija<Form>(reportePostulacionesForm, f => reportePostulacionesForm = f, CrearReportePostulaciones));
            reportesMenuItem.DropDownItems.Add("Ofertas por tipo y estado", null,
                (s, e) => AbrirVentanaHija<Form>(reporteOfertasForm, f => reporteOfertasForm = f, CrearReporteOfertas));

            int posicion = menuStrip1.Items.IndexOf(postulacionesMenuItem!) + 1;
            menuStrip1.Items.Insert(posicion, reportesMenuItem);
        }

        // Configura el formulario genérico con columnas, consulta y series del reporte de postulaciones.
        private static Form CrearReportePostulaciones()
        {
            var client = new ReporteApiClient(ApiHttpClientFactory.Shared);

            return new ReporteForm<ReportePostulacionesOfertaDTO>(
                "Reporte de postulaciones por oferta",
                "Postulaciones por oferta",
                new[]
                {
                    ("Titulo", "Oferta", 260),
                    ("Empresa", "Empresa", 170),
                    ("EstadoOferta", "Estado de la oferta", 120),
                    ("Total", "Total", 60),
                    ("Pendientes", "Pendientes", 80),
                    ("Aceptadas", "Aceptadas", 80),
                    ("Rechazadas", "Rechazadas", 90)
                },
                true,
                (desde, hasta) => client.GetPostulacionesPorOfertaAsync(desde, hasta),
                filas => (
                    filas.Select(f => f.Titulo).ToList(),
                    new List<SerieGrafico>
                    {
                        new("Pendientes", Color.Gray, filas.Select(f => (double)f.Pendientes).ToList()),
                        new("Aceptadas", Color.SeaGreen, filas.Select(f => (double)f.Aceptadas).ToList()),
                        new("Rechazadas", Color.Firebrick, filas.Select(f => (double)f.Rechazadas).ToList())
                    }),
                filas => $"Ofertas: {filas.Count}   |   Postulaciones: {filas.Sum(f => f.Total)} " +
                         $"(pendientes {filas.Sum(f => f.Pendientes)}, aceptadas {filas.Sum(f => f.Aceptadas)}, rechazadas {filas.Sum(f => f.Rechazadas)})");
        }

        // Configura el formulario genérico con columnas, consulta y series del reporte de ofertas por tipo.
        private static Form CrearReporteOfertas()
        {
            var client = new ReporteApiClient(ApiHttpClientFactory.Shared);

            return new ReporteForm<ReporteOfertasTipoDTO>(
                "Reporte de ofertas por tipo y estado",
                "Ofertas por tipo",
                new[]
                {
                    ("TipoOferta", "Tipo de oferta", 300),
                    ("Activas", "Activas", 90),
                    ("NoActivas", "No activas", 90),
                    ("Total", "Total", 90)
                },
                false,
                (desde, hasta) => client.GetOfertasPorTipoAsync(),
                filas => (
                    filas.Select(f => f.TipoOferta).ToList(),
                    new List<SerieGrafico>
                    {
                        new("Activas", Color.SeaGreen, filas.Select(f => (double)f.Activas).ToList()),
                        new("No activas", Color.DarkGray, filas.Select(f => (double)f.NoActivas).ToList())
                    }),
                filas => $"Ofertas: {filas.Sum(f => f.Total)}   |   Activas: {filas.Sum(f => f.Activas)}   |   No activas: {filas.Sum(f => f.NoActivas)}");
        }

        // Configura el listado reutilizable con las operaciones y columnas de postulaciones.
        private static Form CrearListaPostulaciones()
        {
            var client = new PostulacionApiClient(ApiHttpClientFactory.Shared);
            var curriculumClient = new CurriculumApiClient(ApiHttpClientFactory.Shared);

            bool puedeVerCv = Sesion.EsAdministrador || Sesion.EsEmpresa;

            var columnas = new List<(string Propiedad, string Encabezado, int Ancho)>
            {
                ("FechaPostulacion", "Fecha", 90),
                ("AlumnoNombre", "Alumno", 190),
                ("AlumnoLegajo", "Legajo", 70),
                ("AlumnoCarrera", "Carrera", 200),
                ("AlumnoPromedio", "Prom.", 60)
            };
            if (puedeVerCv)
                columnas.Add(("AlumnoTieneCv", "CV", 40));
            columnas.Add(("OfertaTitulo", "Oferta", 220));
            columnas.Add(("EmpresaNombre", "Empresa", 160));
            columnas.Add(("Estado", "Estado", 90));

            var accionesExtra = new List<(string Texto, Func<PostulacionDTO, Task> Accion)>();
            if (puedeVerCv)
            {
                accionesExtra.Add(("Descargar CV", async postulacion =>
                {
                    if (!postulacion.AlumnoTieneCv)
                        throw new InvalidOperationException("El alumno todavía no cargó su currículum.");

                    string nombre = $"CV_{postulacion.AlumnoNombre}.pdf".Replace(", ", "_").Replace(' ', '_');
                    await CurriculumDescarga.DescargarAsync(Form.ActiveForm, curriculumClient, postulacion.AlumnoId, nombre);
                }
                ));
            }

            return new ListaCatalogoForm<PostulacionDTO>(
                Sesion.EsAlumno ? "Mis postulaciones" : "Postulaciones",
                "la postulación",
                columnas,
                () => client.GetAllAsync(),
                postulacion => postulacion.Id,
                postulacion => new PostulacionDetalleForm(postulacion),
                id => client.DeleteAsync(id),
                permiteAgregar: Sesion.EsAdministrador,
                permiteModificar: Sesion.EsEmpresa,
                permiteEliminar: Sesion.EsAdministrador || Sesion.EsAlumno,
                accionesExtra: accionesExtra);
        }

        // Configura el listado reutilizable con las operaciones y columnas de empresas.
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

        // Configura el listado reutilizable con las operaciones y columnas de carreras.
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

        // Configura el listado reutilizable con las operaciones y columnas de tipos de oferta.
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

        // Cierra la sesión y sale de la pantalla principal.
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

                using var login = new LoginForm();
                if (login.ShowDialog(this) == DialogResult.OK)
                    await ActualizarSesionAsync();
                else
                    Close();
            }
        }
    }
}
