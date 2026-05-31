using Moq;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Admin;

public class CambiarRol
{
    [Fact]
    public async Task Usuario_Inexistente_LanzaKeyNotFoundException()
    {
        var mockUsuarios = new Mock<IRepositorioUsuarios>();
        mockUsuarios.Setup(r => r.ObtenerPorIdAsync(999)).ReturnsAsync((Usuario?)null);

        var servicio = new ServicioAdmin(mockUsuarios.Object,
            new Mock<IRepositorioSubastas>().Object, new Mock<IRepositorioRoles>().Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => servicio.CambiarRolUsuarioAsync(999, 1));
    }

    [Fact]
    public async Task Usuario_Existente_RolCambiado()
    {
        var usuario = new Usuario { Id = 1, RolId = 2 };
        var rol = new Rol { Id = 1, Nombre = "administrador" };
        var mockUsuarios = new Mock<IRepositorioUsuarios>();
        mockUsuarios.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);
        mockUsuarios.Setup(r => r.ActualizarAsync(It.IsAny<Usuario>())).ReturnsAsync((Usuario u) => u);
        var mockRoles = new Mock<IRepositorioRoles>();
        mockRoles.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(rol);

        var servicio = new ServicioAdmin(mockUsuarios.Object,
            new Mock<IRepositorioSubastas>().Object, mockRoles.Object);

        await servicio.CambiarRolUsuarioAsync(1, 1);

        Assert.Equal(1, usuario.RolId);
        mockUsuarios.Verify(r => r.ActualizarAsync(It.Is<Usuario>(u => u.RolId == 1)), Times.Once);
    }
}
