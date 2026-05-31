using Moq;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Admin;

public class CancelarSubasta
{
    [Fact]
    public async Task Subasta_Inexistente_LanzaKeyNotFoundException()
    {
        var mockSubastas = new Mock<IRepositorioSubastas>();
        mockSubastas.Setup(r => r.ObtenerPorIdAsync(999)).ReturnsAsync((Subasta?)null);

        var servicio = new ServicioAdmin(new Mock<IRepositorioUsuarios>().Object,
            mockSubastas.Object, new Mock<IRepositorioRoles>().Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => servicio.CancelarSubastaAsync(999));
    }

    [Fact]
    public async Task Subasta_Existente_CambiaACancelada()
    {
        var subasta = new Subasta { Id = 1, Estado = "activa", VendedorId = 5 };
        var mockSubastas = new Mock<IRepositorioSubastas>();
        mockSubastas.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(subasta);
        mockSubastas.Setup(r => r.ActualizarAsync(It.IsAny<Subasta>())).ReturnsAsync((Subasta s) => s);

        var servicio = new ServicioAdmin(new Mock<IRepositorioUsuarios>().Object,
            mockSubastas.Object, new Mock<IRepositorioRoles>().Object);

        await servicio.CancelarSubastaAsync(1);

        Assert.Equal("cancelada", subasta.Estado);
    }
}
