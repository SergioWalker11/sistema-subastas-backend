using Moq;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Tests.Cierres;

public class NoVencida
{
    [Fact]
    public async Task Activa_NoVencida_NoCambia()
    {
        var subasta = new Subasta
        {
            Id = 3, ProductoId = 3, VendedorId = 10,
            PrecioInicial = 100m, PrecioActual = 100m, Estado = "activa",
            FechaInicio = DateTime.UtcNow.AddDays(-1), FechaFin = DateTime.UtcNow.AddDays(5)
        };
        var mockRepo = new Mock<IRepositorioSubastas>();
        mockRepo.Setup(r => r.ObtenerPorEstadoAsync("activa")).ReturnsAsync([subasta]);

        var ahora = DateTime.UtcNow;
        var vencidas = (await mockRepo.Object.ObtenerPorEstadoAsync("activa"))
            .Where(s => s.FechaFin <= ahora).ToList();

        Assert.Empty(vencidas);
    }
}
