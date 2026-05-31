using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Middleware;
using SistemaSubastaBackend.Repositorios;
using SistemaSubastaBackend.Servicios;
using SistemaSubastaBackend.ServiciosExternos;
using SistemaSubastaBackend.Utilidades;

var builder = WebApplication.CreateBuilder(args);

var jwtClave = builder.Configuration["Jwt:Clave"] ?? "ClaveSuperSecretaParaDesarrolloLocal12345678";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Emisor"] ?? "SistemaSubasta",
            ValidAudience = builder.Configuration["Jwt:Audiencia"] ?? "SistemaSubastaClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtClave))
        };
    });

builder.Services.AddAuthorization();

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
builder.Services.AddScoped<IRepositorioProductos, RepositorioProductos>();
builder.Services.AddScoped<IRepositorioCategorias, RepositorioCategorias>();
builder.Services.AddScoped<IRepositorioRoles, RepositorioRoles>();

builder.Services.AddScoped<IServicioSubastas, ServicioSubastas>();
builder.Services.AddScoped<IServicioPujas, ServicioPujas>();
builder.Services.AddScoped<IServicioPagos, ServicioPagos>();
builder.Services.AddScoped<IServicioNotificaciones, ServicioNotificaciones>();
builder.Services.AddScoped<IServicioAutenticacion, ServicioAutenticacion>();
builder.Services.AddScoped<IServicioImagenes, ServicioImagenes>();
builder.Services.AddScoped<IServicioAdmin, ServicioAdmin>();
builder.Services.AddScoped<IServicioProductos, ServicioProductos>();

builder.Services.AddSingleton<IValidadorPujas, ValidadorPujas>();
builder.Services.AddScoped<IServicioPasarelaPagos, ServicioPasarelaPagos>();
builder.Services.AddHostedService<ServicioCierreSubastas>();
builder.Services.AddHostedService<ServicioPagosVencidos>();

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
app.UseMiddleware<MiddlewareExcepciones>();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
