using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI;
using Application.Services;
using Data;
using DTOs;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.Administrador, p => p.RequireRole(Roles.Administrador));
    options.AddPolicy(Policies.AdministradorOEmpresa, p => p.RequireRole(Roles.Administrador, Roles.Empresa));
    options.AddPolicy(Policies.AdministradorOAlumno, p => p.RequireRole(Roles.Administrador, Roles.Alumno));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              // Necesario para que Blazor (navegador) pueda leer el nombre del archivo descargado.
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

app.MapAuthEndpoints();
app.MapAlumnoEndpoints();
app.MapCarreraEndpoints();
app.MapOfertaEndpoints();
app.MapEmpresaEndpoints();
app.MapTipoOfertaEndpoints();
app.MapPostulacionEndpoints();
app.MapReporteEndpoints();
app.MapCurriculumEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BolsaTrabajoContext>();

    // Aplica las migraciones pendientes (crea la base si no existe).
    await db.Database.MigrateAsync();

    try
    {
        await DbSeeder.SeedAsync(db);

        // Datos de prueba (alumnos, ofertas, postulaciones). Se desactivan con "DatosDePrueba": false en appsettings.json.
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
