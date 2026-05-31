using Moq;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Subastas;

public class CambiarEstado
{
    [Fact]
    public async Task Subasta_Inexistente_LanzaKeyNotFoundException()
    {
        var mockRepo = new Mock<IRepositorioSubastas>();
        mockRepo.Setup(r => r.ObtenerPorIdAsync(999)).ReturnsAsync((Subasta?)null);

        var servicio = new ServicioSubastas(mockRepo.Object,
            new Mock<IRepositorioPujas>().Object, new Mock<IRepositorioPagos>().Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => servicio.ActualizarEstadoAsync(999, "cancelada"));
    }

    [Fact]
    public async Task Subasta_Existente_CambiaEstado()
    {
        var subasta = new Subasta { Id = 1, Estado = "activa" };
        var mockRepo = new Mock<IRepositorioSubastas>();
        mockRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(subasta);
        mockRepo.Setup(r => r.ActualizarAsync(It.IsAny<Subasta>())).ReturnsAsync((Subasta s) => s);

        var servicio = new ServicioSubastas(mockRepo.Object,
            new Mock<IRepositorioPujas>().Object, new Mock<IRepositorioPagos>().Object);

        var result = await servicio.ActualizarEstadoAsync(1, "cancelada");

        Assert.Equal("cancelada", result.Estado);
    }
}
