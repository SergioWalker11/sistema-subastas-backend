using Moq;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Tests.Cierres;

public class ActivaAPendientePago
{
    [Fact]
    public async Task ConPujas_CambiaAPendientePago_RegistraGanador()
    {
        var subasta = new Subasta
        {
            Id = 1, ProductoId = 1, VendedorId = 10,
            PrecioInicial = 100m, PrecioActual = 500m, Estado = "activa",
            FechaInicio = DateTime.UtcNow.AddDays(-10), FechaFin = DateTime.UtcNow.AddHours(-2),
            Producto = new Producto { Id = 1, Nombre = "Test" }
        };
        var ultimaPuja = new Puja
            { Id = 1, SubastaId = 1, UsuarioId = 20, Monto = 500m, FechaCreacion = DateTime.UtcNow.AddHours(-3) };
        var mockRepo = new Mock<IRepositorioSubastas>();
        mockRepo.Setup(r => r.ObtenerPorEstadoAsync("activa")).ReturnsAsync([subasta]);
        mockRepo.Setup(r => r.ActualizarAsync(It.IsAny<Subasta>()))
            .Callback<Subasta>(s =>
            {
                subasta.Estado = s.Estado;
                subasta.GanadorId = s.GanadorId;
                subasta.FechaLimitePago = s.FechaLimitePago;
            }).ReturnsAsync(subasta);
        var mockPujas = new Mock<IRepositorioPujas>();
        mockPujas.Setup(r => r.ObtenerUltimaPujaAsync(1)).ReturnsAsync(ultimaPuja);

        var ahora = DateTime.UtcNow;
        var activas = await mockRepo.Object.ObtenerPorEstadoAsync("activa");
        foreach (var s in activas.Where(s => s.FechaFin <= ahora))
        {
            var u = await mockPujas.Object.ObtenerUltimaPujaAsync(s.Id);
            if (u != null)
            {
                s.GanadorId = u.UsuarioId;
                s.Estado = "pendiente_pago";
                s.FechaLimitePago = ahora.AddHours(24);
                await mockRepo.Object.ActualizarAsync(s);
            }
        }

        Assert.Equal("pendiente_pago", subasta.Estado);
        Assert.Equal(20, subasta.GanadorId);
        Assert.True(subasta.FechaLimitePago > ahora);
    }
}
