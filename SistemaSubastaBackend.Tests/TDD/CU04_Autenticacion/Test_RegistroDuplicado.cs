using Moq;
using Microsoft.Extensions.Configuration;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Autenticacion;

public class RegistroDuplicado
{
    [Fact]
    public async Task Correo_Duplicado_LanzaExcepcion()
    {
        var existente = new Usuario { Id = 1, Correo = "existe@email.com" };
        var mockUsuarios = new Mock<IRepositorioUsuarios>();
        mockUsuarios.Setup(r => r.ObtenerPorCorreoAsync("existe@email.com")).ReturnsAsync(existente);

        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Jwt:Clave"] = "ClaveSuperSecretaDeAlMenos32Caracteres!!"
        }).Build();

        var servicio = new ServicioAutenticacion(mockUsuarios.Object, config);
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => servicio.RegistroAsync(new RegistroDTO
            {
                NombreCompleto = "X", Correo = "existe@email.com", Contrasena = "password", RolId = 2
            }));
        Assert.Contains("registrado", ex.Message.ToLower());
    }
}
