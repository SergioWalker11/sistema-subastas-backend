using Moq;
using Microsoft.Extensions.Configuration;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Autenticacion;

public class LoginValido
{
    [Fact]
    public async Task Credenciales_Correctas_RetornaToken()
    {
        var usuario = new Usuario
        {
            Id = 1, NombreCompleto = "Carlos", Correo = "carlos@email.com",
            ContrasenaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Rol = new Rol { Nombre = "comprador" }
        };
        var mockUsuarios = new Mock<IRepositorioUsuarios>();
        mockUsuarios.Setup(r => r.ObtenerPorCorreoAsync("carlos@email.com")).ReturnsAsync(usuario);

        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Jwt:Clave"] = "ClaveSuperSecretaDeAlMenos32Caracteres!!",
            ["Jwt:Emisor"] = "TestIssuer",
            ["Jwt:Audiencia"] = "TestAudience"
        }).Build();

        var servicio = new ServicioAutenticacion(mockUsuarios.Object, config);
        var result = await servicio.LoginAsync(new LoginDTO { Correo = "carlos@email.com", Contrasena = "123456" });

        Assert.NotNull(result.Token);
        Assert.Equal("Carlos", result.NombreCompleto);
        Assert.Equal("comprador", result.Rol);
    }
}
