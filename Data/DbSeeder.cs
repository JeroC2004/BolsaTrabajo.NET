using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    /// <summary>
    /// Datos de prueba que no pueden viajar en las migraciones: usuarios con contraseña hasheada
    /// (sal aleatoria) y alumnos vinculados a un usuario. Es idempotente: se puede ejecutar en
    /// cada arranque de la API sin duplicar nada.
    /// </summary>
    public static class DbSeeder
    {
        private const string PasswordEmpresa = "empresa123";
        private const string PasswordAlumno = "alumno123";

        public static async Task SeedAsync(BolsaTrabajoContext context)
        {
            // Un usuario creado antes de que existieran los roles queda con la columna Rol vacía.
            // El único que existía era el administrador, así que se le asigna ese rol.
            await context.Database.ExecuteSqlRawAsync(
                "UPDATE Usuarios SET Rol = 'Administrador' WHERE Rol IS NULL OR Rol = ''");

            await AsegurarUsuarioAsync(context, "admin", "admin@bolsatrabajo.com", "admin123", RolUsuario.Administrador);

            // Las empresas (Ids 1 a 3) se cargan por migración.
            await AsegurarUsuarioEmpresaAsync(context, 1, "techcorp", "rrhh@techcorp.com");
            await AsegurarUsuarioEmpresaAsync(context, 2, "datasoft", "rrhh@datasoft.com");
            await AsegurarUsuarioEmpresaAsync(context, 3, "nubecloud", "rrhh@nubecloud.com");

            await AsegurarAlumnoConUsuarioAsync(context, "52001", "Lucía", "Fernández",
                "lucia.fernandez@alumnos.utn.edu.ar", "40123456", "2023", 4, 28, 8.2f, 1, "lucia");
            await AsegurarAlumnoConUsuarioAsync(context, "52002", "Mateo", "Gómez",
                "mateo.gomez@alumnos.utn.edu.ar", "41234567", "2023", 3, 20, 7.5f, 1, "mateo");
        }

        // ------------------------------------------------------------------------------------
        // Datos de prueba: alumnos de distintas carreras, ofertas de las tres empresas en
        // distintos estados (con requisitos) y postulaciones en los tres estados repartidas en
        // el tiempo. Se cargan una sola vez, en una transacción: si la primera oferta de prueba
        // ya existe, no se vuelve a cargar nada.
        // ------------------------------------------------------------------------------------
        private const string PrimeraOfertaDemo = "Pasante Desarrollador .NET";

        private sealed record AlumnoDemo(string Legajo, string Nombre, string Apellido, string Email, string Dni,
            int Anio, int Materias, float Promedio, int CarreraId, string Usuario);

        private sealed record OfertaDemo(string Clave, string Titulo, TipoVinculo Vinculo, int DesdeDias, int HastaDias,
            string Detalle, EstadoOferta Estado, int EmpresaId, int TipoOfertaId, (string Texto, bool Excluyente)[] Requisitos);

        private sealed record PostulacionDemo(string Legajo, string Oferta, int DiasAtras, EstadoPostulacion Estado);

        // Los alumnos 52001 (lucia) y 52002 (mateo) los carga SeedAsync.
        private static readonly AlumnoDemo[] AlumnosDemo =
        {
            new("52003", "Sofía", "Martínez", "sofia.martinez@alumnos.utn.edu.ar", "40876543", 5, 36, 8.9f, 1, "sofia"),
            new("52004", "Tomás", "Rodríguez", "tomas.rodriguez@alumnos.utn.edu.ar", "41567890", 2, 12, 6.8f, 1, "tomas"),
            new("52005", "Camila", "López", "camila.lopez@alumnos.utn.edu.ar", "40345678", 4, 27, 9.1f, 1, "camila"),
            new("52006", "Nicolás", "Pérez", "nicolas.perez@alumnos.utn.edu.ar", "41098765", 3, 19, 7.2f, 2, "nicolas"),
            new("52007", "Valentina", "Romero", "valentina.romero@alumnos.utn.edu.ar", "40765432", 4, 26, 8.4f, 3, "valentina"),
            new("52008", "Franco", "Benítez", "franco.benitez@alumnos.utn.edu.ar", "41456789", 5, 35, 7.9f, 4, "franco"),
            new("52009", "Julieta", "Acosta", "julieta.acosta@alumnos.utn.edu.ar", "40654321", 2, 11, 8.0f, 4, "julieta"),
            new("52010", "Agustín", "Suárez", "agustin.suarez@alumnos.utn.edu.ar", "41321098", 3, 18, 6.5f, 5, "agustin"),
        };

        // Las fechas son relativas a hoy (negativo: pasado), así las ofertas activas siguen vigentes.
        private static readonly OfertaDemo[] OfertasDemo =
        {
            // TechCorp S.A.
            new("a", PrimeraOfertaDemo, TipoVinculo.Pasantia, -20, 40,
                "Pasantía en el equipo de desarrollo de productos web. Se trabaja con metodologías ágiles junto a desarrolladores senior.",
                EstadoOferta.Activa, 1, 1, new[]
                {
                    ("Cursando 3.er año o superior de Ingeniería en Sistemas de Información", true),
                    ("Conocimientos de C# y programación orientada a objetos", true),
                    ("Manejo de Git", false),
                    ("Inglés técnico de lectura", false)
                }),
            new("b", "Desarrollador Backend Junior", TipoVinculo.EmpleoFullTime, -10, 30,
                "Desarrollo y mantenimiento de APIs REST sobre .NET para clientes del sector financiero.",
                EstadoOferta.Activa, 1, 4, new[]
                {
                    ("Experiencia mínima de 1 año con .NET", true),
                    ("SQL Server y Entity Framework", false),
                    ("Diseño de APIs REST", false)
                }),
            new("c", "Analista de Calidad (QA) part-time", TipoVinculo.EmpleoPartTime, -5, 25,
                "Diseño y ejecución de casos de prueba manuales y automatizados. Horario flexible, 20 horas semanales.",
                EstadoOferta.Activa, 1, 3, new[]
                {
                    ("Conocimientos de pruebas manuales y automatizadas", true),
                    ("Disponibilidad de 20 horas semanales", true)
                }),
            new("d", "Pasante de Soporte Técnico", TipoVinculo.Pasantia, -90, -30,
                "Atención de incidentes de primer nivel y soporte a usuarios internos. Búsqueda ya cerrada.",
                EstadoOferta.Cerrada, 1, 1, new[]
                {
                    ("Disponibilidad horaria por la tarde", true)
                }),

            // DataSoft Rosario S.A.
            new("e", "Primer empleo: Analista de Datos", TipoVinculo.PrimerEmpleo, -15, 45,
                "Armado de tableros e indicadores para clientes de distintos rubros. Ideal para quien busca su primera experiencia laboral.",
                EstadoOferta.Activa, 2, 2, new[]
                {
                    ("Estudiante avanzado o graduado reciente", true),
                    ("Manejo de SQL", true),
                    ("Conocimientos de Power BI o herramientas similares", false)
                }),
            new("f", "Desarrollador Full Stack", TipoVinculo.EmpleoFullTime, -7, 60,
                "Desarrollo de aplicaciones web a medida: backend en .NET y frontend con frameworks modernos.",
                EstadoOferta.Activa, 2, 4, new[]
                {
                    ("Experiencia con JavaScript y frameworks modernos", true),
                    ("Experiencia con .NET", false),
                    ("Trabajo en equipo con metodologías ágiles", false)
                }),
            new("g", "Pasante de Consultoría IT", TipoVinculo.Pasantia, -30, 30,
                "Acompañamiento a consultores en relevamientos y documentación de procesos. Búsqueda en pausa hasta nuevo aviso.",
                EstadoOferta.Pausada, 2, 1, new[]
                {
                    ("Cursando Ingeniería en Sistemas o Industrial", true),
                    ("Buena comunicación oral y escrita", false)
                }),
            new("h", "Practicante de Mejora de Procesos", TipoVinculo.PracticaProfesional, -12, 35,
                "Relevamiento y optimización de procesos internos y de clientes. Práctica profesional supervisada.",
                EstadoOferta.Activa, 2, 1, new[]
                {
                    ("Cursando Ingeniería Industrial", true),
                    ("Conocimientos de Excel avanzado", false)
                }),

            // NubeCloud Argentina
            new("i", "Pasante DevOps", TipoVinculo.Pasantia, -18, 50,
                "Automatización de despliegues y monitoreo de infraestructura en la nube.",
                EstadoOferta.Activa, 3, 1, new[]
                {
                    ("Conocimientos de Linux", true),
                    ("Nociones de Docker y contenedores", false),
                    ("Interés en infraestructura cloud", false)
                }),
            new("j", "Analista de Ciberseguridad Junior", TipoVinculo.PrimerEmpleo, -3, 55,
                "Monitoreo de eventos de seguridad y respuesta a incidentes junto al equipo de SOC.",
                EstadoOferta.Activa, 3, 2, new[]
                {
                    ("Conocimientos de redes y seguridad informática", true),
                    ("Certificaciones de seguridad (deseable)", false)
                }),
            new("k", "Ingeniero de Infraestructura Cloud", TipoVinculo.EmpleoFullTime, -120, -20,
                "Diseño y operación de infraestructura en AWS y Azure. Búsqueda finalizada: el puesto ya fue cubierto.",
                EstadoOferta.Finalizada, 3, 4, new[]
                {
                    ("Experiencia comprobable en AWS o Azure", true)
                }),
            new("l", "Administrador de Sistemas part-time", TipoVinculo.EmpleoPartTime, -9, 20,
                "Administración de servidores Windows y Linux. Cuatro horas diarias, modalidad híbrida.",
                EstadoOferta.Activa, 3, 3, new[]
                {
                    ("Disponibilidad de 4 horas diarias", true),
                    ("Conocimientos de Windows Server y Linux", false)
                }),
        };

        // Siempre posteriores al inicio de la oferta (DiasAtras no supera el inicio de la oferta).
        private static readonly PostulacionDemo[] PostulacionesDemo =
        {
            new("52001", "a", 8, EstadoPostulacion.Pendiente),
            new("52001", "b", 6, EstadoPostulacion.Aceptada),
            new("52001", "e", 4, EstadoPostulacion.Pendiente),
            new("52001", "i", 12, EstadoPostulacion.Pendiente),

            new("52002", "a", 15, EstadoPostulacion.Aceptada),
            new("52002", "c", 3, EstadoPostulacion.Pendiente),
            new("52002", "d", 60, EstadoPostulacion.Rechazada),

            new("52003", "b", 9, EstadoPostulacion.Aceptada),
            new("52003", "f", 5, EstadoPostulacion.Pendiente),
            new("52003", "k", 80, EstadoPostulacion.Aceptada),
            new("52003", "j", 2, EstadoPostulacion.Pendiente),

            new("52004", "a", 18, EstadoPostulacion.Pendiente),
            new("52004", "c", 4, EstadoPostulacion.Rechazada),
            new("52004", "l", 7, EstadoPostulacion.Pendiente),

            new("52005", "b", 7, EstadoPostulacion.Pendiente),
            new("52005", "e", 10, EstadoPostulacion.Aceptada),
            new("52005", "f", 6, EstadoPostulacion.Aceptada),
            new("52005", "i", 15, EstadoPostulacion.Aceptada),
            new("52005", "j", 1, EstadoPostulacion.Pendiente),

            new("52006", "h", 9, EstadoPostulacion.Pendiente),
            new("52006", "l", 8, EstadoPostulacion.Rechazada),

            new("52007", "e", 13, EstadoPostulacion.Pendiente),
            new("52007", "h", 11, EstadoPostulacion.Aceptada),

            new("52008", "h", 10, EstadoPostulacion.Aceptada),
            new("52008", "g", 25, EstadoPostulacion.Pendiente),
            new("52008", "k", 100, EstadoPostulacion.Rechazada),

            new("52009", "h", 5, EstadoPostulacion.Pendiente),
            new("52009", "g", 20, EstadoPostulacion.Rechazada),
            new("52009", "e", 8, EstadoPostulacion.Pendiente),

            new("52010", "l", 6, EstadoPostulacion.Pendiente),
            new("52010", "i", 17, EstadoPostulacion.Rechazada),
        };

        public static async Task SeedDatosDePruebaAsync(BolsaTrabajoContext context)
        {
            if (await context.Ofertas.AnyAsync(o => o.Titulo == PrimeraOfertaDemo && o.EmpresaId == 1))
                return;

            await using var transaccion = await context.Database.BeginTransactionAsync();
            DateTime hoy = DateTime.Today;

            // 1) Alumnos (cada uno con su usuario).
            foreach (var a in AlumnosDemo)
            {
                await AsegurarAlumnoConUsuarioAsync(context, a.Legajo, a.Nombre, a.Apellido, a.Email, a.Dni,
                    "2023", a.Anio, a.Materias, a.Promedio, a.CarreraId, a.Usuario);
            }

            // 2) Ofertas con sus requisitos (cabecera y líneas se guardan juntas).
            var ofertas = new Dictionary<string, Oferta>();
            foreach (var d in OfertasDemo)
            {
                var oferta = await context.Ofertas.FirstOrDefaultAsync(o => o.Titulo == d.Titulo && o.EmpresaId == d.EmpresaId);

                if (oferta == null)
                {
                    oferta = new Oferta(0, d.Titulo, d.Vinculo, hoy.AddDays(d.DesdeDias), hoy.AddDays(d.HastaDias),
                        d.Detalle, d.Estado, d.EmpresaId, d.TipoOfertaId);
                    oferta.SetRequisitos(d.Requisitos
                        .Select(r => new RequisitoOferta { Descripcion = r.Texto, EsExcluyente = r.Excluyente })
                        .ToList());
                    context.Ofertas.Add(oferta);
                }

                ofertas[d.Clave] = oferta;
            }
            await context.SaveChangesAsync();

            // 3) Postulaciones.
            string[] legajos = PostulacionesDemo.Select(p => p.Legajo).Distinct().ToArray();
            var alumnos = await context.Alumnos
                .Where(a => legajos.Contains(a.Legajo))
                .ToDictionaryAsync(a => a.Legajo);

            var existentes = (await context.Postulaciones
                    .Select(p => new { p.AlumnoId, p.OfertaId })
                    .ToListAsync())
                .Select(p => (p.AlumnoId, p.OfertaId))
                .ToHashSet();

            foreach (var p in PostulacionesDemo)
            {
                var alumno = alumnos[p.Legajo];
                var oferta = ofertas[p.Oferta];

                if (!existentes.Add((alumno.Id, oferta.Id)))
                    continue;

                context.Postulaciones.Add(new Postulacion(0, alumno.Id, oferta.Id, hoy.AddDays(-p.DiasAtras), p.Estado));
            }
            await context.SaveChangesAsync();

            await transaccion.CommitAsync();
        }

        // ------------------------------------------------------------------------------------
        // CV de prueba: un PDF sencillo para cada alumno de demostración. Se cargan una sola vez
        // (si ya hay algún CV en la base no se toca nada) para no pisar CV reales ni volver a
        // crear uno que un alumno haya borrado a propósito.
        // ------------------------------------------------------------------------------------
        public static async Task SeedCurriculumsDePruebaAsync(BolsaTrabajoContext context)
        {
            if (await context.Curriculums.AnyAsync())
                return;

            var legajosDemo = AlumnosDemo.Select(a => a.Legajo).Concat(new[] { "52001", "52002" }).ToList();

            var alumnos = await context.Alumnos
                .Include(a => a.Carrera)
                .Where(a => legajosDemo.Contains(a.Legajo))
                .ToListAsync();

            foreach (var alumno in alumnos)
            {
                byte[] pdf = PdfDemo.Crear(
                    $"{alumno.NomAlumno} {alumno.ApeAlumno} - Curriculum Vitae",
                    new[]
                    {
                        $"Legajo: {alumno.Legajo}",
                        $"Email: {alumno.Email}",
                        $"Carrera: {alumno.Carrera?.NomCarrera}  (anio de cursado: {alumno.AnioCurso})",
                        $"Materias aprobadas: {alumno.CantMatAp}   Promedio: {alumno.Promedio:0.0}",
                        "",
                        "Perfil",
                        "Estudiante universitario con interes en sumar experiencia profesional",
                        "a traves de pasantias y primeros empleos.",
                        "",
                        "Habilidades",
                        "Trabajo en equipo, resolucion de problemas, comunicacion.",
                        "",
                        "Idiomas",
                        "Espanol (nativo), Ingles (intermedio).",
                        "",
                        "CV generado automaticamente como dato de prueba."
                    });

                string nombreArchivo = $"CV_{alumno.ApeAlumno}_{alumno.NomAlumno}.pdf".Replace(' ', '_');
                context.Curriculums.Add(new CurriculumVitae(0, alumno.Id, nombreArchivo, pdf, DateTime.Now));
            }

            await context.SaveChangesAsync();
        }

        private static async Task AsegurarUsuarioEmpresaAsync(BolsaTrabajoContext context, int empresaId, string username, string email)
        {
            if (!await context.Empresas.AnyAsync(e => e.Id == empresaId))
                return;

            await AsegurarUsuarioAsync(context, username, email, PasswordEmpresa, RolUsuario.Empresa, empresaId: empresaId);
        }

        private static async Task AsegurarAlumnoConUsuarioAsync(BolsaTrabajoContext context, string legajo, string nombre,
            string apellido, string email, string dni, string plan, int anioCurso, int materiasAprobadas,
            float promedio, int carreraId, string username)
        {
            if (!await context.Carreras.AnyAsync(c => c.Id == carreraId))
                return;

            var alumno = await context.Alumnos.FirstOrDefaultAsync(a => a.Legajo == legajo);
            if (alumno == null)
            {
                alumno = new Alumno(0, nombre, apellido, email, legajo, dni, plan, anioCurso,
                    materiasAprobadas, promedio, carreraId, DateTime.Today);
                context.Alumnos.Add(alumno);
                await context.SaveChangesAsync();
            }

            await AsegurarUsuarioAsync(context, username, email, PasswordAlumno, RolUsuario.Alumno, alumnoId: alumno.Id);
        }

        private static async Task AsegurarUsuarioAsync(BolsaTrabajoContext context, string username, string email,
            string password, RolUsuario rol, int? alumnoId = null, int? empresaId = null)
        {
            var existente = await context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);

            if (existente == null)
            {
                context.Usuarios.Add(new Usuario(0, username, email, password, DateTime.UtcNow, true, rol, alumnoId, empresaId));
                await context.SaveChangesAsync();
                return;
            }

            if (existente.Rol != rol || existente.AlumnoId != alumnoId || existente.EmpresaId != empresaId || !existente.Activo)
            {
                existente.SetRol(rol, alumnoId, empresaId);
                existente.SetActivo(true);
                await context.SaveChangesAsync();
            }
        }
    }
}
