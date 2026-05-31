using Moq;
using Microsoft.Extensions.Configuration;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Autenticacion;

public class LoginInvalido
{
    [Fact]
    public async Task Credenciales_Incorrectas_LanzaExcepcion()
    {
        var usuario = new Usuario
        {
            Id = 1, NombreCompleto = "Carlos", Correo = "carlos@email.com",
            ContrasenaHash = BCrypt.Net.BCrypt.HashPassword("123456")
        };
        var mockUsuarios = new Mock<IRepositorioUsuarios>();
        mockUsuarios.Setup(r => r.ObtenerPorCorreoAsync("carlos@email.com")).ReturnsAsync(usuario);

        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Jwt:Clave"] = "ClaveSuperSecretaDeAlMenos32Caracteres!!"
        }).Build();

        var servicio = new ServicioAutenticacion(mockUsuarios.Object, config);
        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => servicio.LoginAsync(new LoginDTO { Correo = "carlos@email.com", Contrasena = "wrong" }));
        Assert.Contains("Credenciales", ex.Message);
    }
}
