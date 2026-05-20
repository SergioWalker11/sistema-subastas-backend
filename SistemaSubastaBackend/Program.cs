using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Repositorios;
using SistemaSubastaBackend.Servicios;
using SistemaSubastaBackend.ServiciosExternos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContextoSubastas>(opciones =>
    opciones.UseNpgsql(builder.Configuration["CadenasConexion:ConexionPostgreSQL"]));

builder.Services.AddScoped<IRepositorioSubastas, RepositorioSubastas>();
builder.Services.AddScoped<IRepositorioPujas, RepositorioPujas>();
builder.Services.AddScoped<IRepositorioPagos, RepositorioPagos>();
builder.Services.AddScoped<IRepositorioNotificaciones, RepositorioNotificaciones>();
builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();

builder.Services.AddScoped<IServicioSubastas, ServicioSubastas>();
builder.Services.AddScoped<IServicioPujas, ServicioPujas>();
builder.Services.AddScoped<IServicioPagos, ServicioPagos>();
builder.Services.AddScoped<IServicioNotificaciones, ServicioNotificaciones>();

builder.Services.AddSingleton<ServicioPasarelaPagos>();

builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("PoliticaCors", politica =>
    {
        politica.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var alcance = app.Services.CreateScope())
{
    var contexto = alcance.ServiceProvider.GetRequiredService<ContextoSubastas>();
    contexto.Database.Migrate();
    SembradorDatos.Sembrar(contexto);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PoliticaCors");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
