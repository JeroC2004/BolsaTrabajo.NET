using WebAPI;
using Application.Services;
using Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Dependency Injection - Repositorios (persistencia en memoria)
builder.Services.AddScoped<IAlumnoRepository, AlumnoRepository>();
builder.Services.AddScoped<ICarreraRepository, CarreraRepository>();
builder.Services.AddScoped<IOfertaRepository, OfertaRepository>();
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<ITipoOfertaRepository, TipoOfertaRepository>();

// Add Dependency Injection - Servicios
builder.Services.AddScoped<IAlumnoService, AlumnoService>();
builder.Services.AddScoped<ICarreraService, CarreraService>();
builder.Services.AddScoped<IOfertaService, OfertaService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<ITipoOfertaService, TipoOfertaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Map endpoints
app.MapAlumnoEndpoints();
app.MapCarreraEndpoints();
app.MapOfertaEndpoints();
app.MapEmpresaEndpoints();
app.MapTipoOfertaEndpoints();

app.Run();
