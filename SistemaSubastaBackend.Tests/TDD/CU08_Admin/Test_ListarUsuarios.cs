using Moq;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Admin;

public class ListarUsuarios
{
    [Fact]
    public async Task Retorna_TodosLosUsuarios()
    {
        var usuarios = new List<Modelos.Usuario>
        {
            new() { Id = 1, NombreCompleto = "Admin", Correo = "admin@t.com", RolId = 1, Rol = new Modelos.Rol { Nombre = "administrador" } },
            new() { Id = 2, NombreCompleto = "Carlos", Correo = "c@t.com", RolId = 2, Rol = new Modelos.Rol { Nombre = "comprador" } }
        };
        var mockUsuarios = new Mock<IRepositorioUsuarios>();
        mockUsuarios.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(usuarios);

        var servicio = new ServicioAdmin(mockUsuarios.Object,
            new Mock<IRepositorioSubastas>().Object, new Mock<IRepositorioRoles>().Object);

        var result = await servicio.ListarUsuariosAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.RolNombre == "administrador");
        Assert.Contains(result, u => u.RolNombre == "comprador");
    }
}
