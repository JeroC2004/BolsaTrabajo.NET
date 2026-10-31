// Guía de estudio: Inicio de la API: registra contexto SQL Server, repositorios y servicios por inyección de dependencias; configura Swagger, JWT, políticas y CORS; publica endpoints, aplica migraciones y carga datos iniciales.
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI;
using Application.Services;
using Data;
using DTOs;

var builder = WebApplication.CreateBuilder(args);

// Swagger describe los endpoints y permite probarlos enviando el JWT desde su interfaz.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Ingresar el token JWT. Ejemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// El contenedor crea el contexto SQL Server y entrega las dependencias a cada petición.
builder.Services.AddDbContext<BolsaTrabajoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAlumnoRepository, AlumnoRepository>();
builder.Services.AddScoped<ICarreraRepository, CarreraRepository>();
builder.Services.AddScoped<IOfertaRepository, OfertaRepository>();
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<ITipoOfertaRepository, TipoOfertaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPostulacionRepository, PostulacionRepository>();
builder.Services.AddScoped<IReporteRepository, ReporteRepository>();
builder.Services.AddScoped<ICurriculumRepository, CurriculumRepository>();

builder.Services.AddScoped<IAlumnoService, AlumnoService>();
builder.Services.AddScoped<ICarreraService, CarreraService>();
builder.Services.AddScoped<IOfertaService, OfertaService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<ITipoOfertaService, TipoOfertaService>();
builder.Services.AddScoped<IPostulacionService, PostulacionService>();
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<ICurriculumService, CurriculumService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Estos parámetros deben coincidir con los utilizados por AuthService al firmar el token.
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

// Una política agrupa los roles que pueden ejecutar una acción protegida de la API.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.Administrador,
        p => p.RequireRole(Roles.Administrador));

    options.AddPolicy(Policies.Empresa,
        p => p.RequireRole(Roles.Empresa));

    options.AddPolicy(Policies.AdministradorOEmpresa,
        p => p.RequireRole(Roles.Administrador, Roles.Empresa));

    options.AddPolicy(Policies.AdministradorOAlumno,
        p => p.RequireRole(Roles.Administrador, Roles.Alumno));
});

// CORS permite que el navegador consuma la API desde un origen diferente al del servidor.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("Content-Disposition");
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Cada extensión registra las rutas HTTP de un recurso; la lógica está en sus servicios.
app.MapAuthEndpoints();
app.MapAlumnoEndpoints();
app.MapCarreraEndpoints();
app.MapOfertaEndpoints();
app.MapEmpresaEndpoints();
app.MapTipoOfertaEndpoints();
app.MapPostulacionEndpoints();
app.MapReporteEndpoints();
app.MapCurriculumEndpoints();

// El scope permite resolver el contexto fuera de una petición y liberarlo al terminar.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BolsaTrabajoContext>();

    // Actualiza el esquema antes de intentar insertar los datos iniciales o de prueba.
    await db.Database.MigrateAsync();

    try
    {
        await DbSeeder.SeedAsync(db);

        if (app.Configuration.GetValue("DatosDePrueba", true))
        {
            await DbSeeder.SeedDatosDePruebaAsync(db);
            await DbSeeder.SeedCurriculumsDePruebaAsync(db);
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "No se pudieron cargar los datos de prueba.");
    }
}

app.Run();
