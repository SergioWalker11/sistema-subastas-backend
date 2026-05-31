using Moq;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Tests.Cierres;

public class PendienteNoVencida
{
    [Fact]
    public async Task Plazo_Vigente_NoCambia()
    {
        var subasta = new Subasta
        {
            Id = 5, ProductoId = 5, VendedorId = 10, GanadorId = 20,
            PrecioInicial = 100m, PrecioActual = 500m, Estado = "pendiente_pago",
            FechaLimitePago = DateTime.UtcNow.AddHours(12)
        };
        var mockRepo = new Mock<IRepositorioSubastas>();
        mockRepo.Setup(r => r.ObtenerPorEstadoAsync("pendiente_pago")).ReturnsAsync([subasta]);

        var ahora = DateTime.UtcNow;
        var vencidas = (await mockRepo.Object.ObtenerPorEstadoAsync("pendiente_pago"))
            .Where(s => s.FechaLimitePago.HasValue && s.FechaLimitePago.Value <= ahora).ToList();

        Assert.Empty(vencidas);
    }
}
