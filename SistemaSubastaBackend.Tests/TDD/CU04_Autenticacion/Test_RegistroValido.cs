using Moq;
using Microsoft.Extensions.Configuration;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Autenticacion;

public class RegistroValido
{
    [Fact]
    public async Task Correo_Nuevo_RegistraUsuario()
    {
        var mockUsuarios = new Mock<IRepositorioUsuarios>();
        mockUsuarios.Setup(r => r.ObtenerPorCorreoAsync("nuevo@email.com")).ReturnsAsync((Usuario?)null);
        mockUsuarios.Setup(r => r.CrearAsync(It.IsAny<Usuario>())).ReturnsAsync((Usuario u) =>
        {
            u.Id = 5;
            u.Rol = new Rol { Nombre = "comprador" };
            return u;
        });

        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Jwt:Clave"] = "ClaveSuperSecretaDeAlMenos32Caracteres!!",
            ["Jwt:Emisor"] = "Test", ["Jwt:Audiencia"] = "Test"
        }).Build();

        var servicio = new ServicioAutenticacion(mockUsuarios.Object, config);
        var result = await servicio.RegistroAsync(new RegistroDTO
        {
            NombreCompleto = "Nuevo Usuario", Correo = "nuevo@email.com",
            Contrasena = "password", RolId = 2
        });

        Assert.NotNull(result.Token);
        Assert.Equal("Nuevo Usuario", result.NombreCompleto);
    }
}
