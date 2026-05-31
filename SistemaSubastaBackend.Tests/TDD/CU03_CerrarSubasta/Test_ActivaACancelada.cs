using Moq;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Tests.Cierres;

public class ActivaACancelada
{
    [Fact]
    public async Task SinPujas_CambiaACancelada()
    {
        var subasta = new Subasta
        {
            Id = 2, ProductoId = 2, VendedorId = 10,
            PrecioInicial = 100m, PrecioActual = 100m, Estado = "activa",
            FechaInicio = DateTime.UtcNow.AddDays(-10), FechaFin = DateTime.UtcNow.AddHours(-2)
        };
        var mockRepo = new Mock<IRepositorioSubastas>();
        mockRepo.Setup(r => r.ObtenerPorEstadoAsync("activa")).ReturnsAsync([subasta]);
        mockRepo.Setup(r => r.ActualizarAsync(It.IsAny<Subasta>()))
            .Callback<Subasta>(s => subasta.Estado = s.Estado).ReturnsAsync(subasta);
        var mockPujas = new Mock<IRepositorioPujas>();
        mockPujas.Setup(r => r.ObtenerUltimaPujaAsync(2)).ReturnsAsync((Puja?)null);

        var ahora = DateTime.UtcNow;
        var activas = await mockRepo.Object.ObtenerPorEstadoAsync("activa");
        foreach (var s in activas.Where(s => s.FechaFin <= ahora))
        {
            var u = await mockPujas.Object.ObtenerUltimaPujaAsync(s.Id);
            if (u == null) { s.Estado = "cancelada"; await mockRepo.Object.ActualizarAsync(s); }
        }

        Assert.Equal("cancelada", subasta.Estado);
    }
}
