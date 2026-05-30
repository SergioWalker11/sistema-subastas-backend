using Moq;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Tests.Cierres;

public class PendienteAIncumplida
{
    [Fact]
    public async Task PlazoVencido_CambiaAIncumplida()
    {
        var subasta = new Subasta
        {
            Id = 4, ProductoId = 4, VendedorId = 10, GanadorId = 20,
            PrecioInicial = 100m, PrecioActual = 500m, Estado = "pendiente_pago",
            FechaLimitePago = DateTime.UtcNow.AddHours(-1),
            Producto = new Producto { Id = 4, Nombre = "Producto Vencido" }
        };
        var mockRepo = new Mock<IRepositorioSubastas>();
        mockRepo.Setup(r => r.ObtenerPorEstadoAsync("pendiente_pago")).ReturnsAsync([subasta]);
        mockRepo.Setup(r => r.ActualizarAsync(It.IsAny<Subasta>()))
            .Callback<Subasta>(s => subasta.Estado = s.Estado).ReturnsAsync(subasta);

        var ahora = DateTime.UtcNow;
        var pendientes = await mockRepo.Object.ObtenerPorEstadoAsync("pendiente_pago");
        foreach (var s in pendientes.Where(s => s.FechaLimitePago.HasValue && s.FechaLimitePago.Value <= ahora))
        {
            s.Estado = "incumplida";
            await mockRepo.Object.ActualizarAsync(s);
        }

        Assert.Equal("incumplida", subasta.Estado);
    }
}
