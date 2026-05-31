using Moq;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;
using SistemaSubastaBackend.ServiciosExternos;

namespace SistemaSubastaBackend.Tests.Pagos;

public class Ganador
{
    [Fact]
    public async Task Usuario_NoEsGanador_LanzaExcepcion()
    {
        var subasta = new Subasta
        {
            Id = 1, Estado = "pendiente_pago", GanadorId = 10, VendedorId = 20,
            PrecioActual = 500m, FechaLimitePago = DateTime.UtcNow.AddHours(12),
            Producto = new Producto { Nombre = "Test" }
        };
        var mockSubastas = new Mock<IRepositorioSubastas>();
        mockSubastas.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(subasta);
        var mockUsuarios = new Mock<IRepositorioUsuarios>();
        mockUsuarios.Setup(r => r.ObtenerPorIdAsync(5)).ReturnsAsync(new Usuario
            { Id = 5, NombreCompleto = "O", Correo = "o@t.com", Rol = new Rol { Nombre = "comprador" } });
        var mockNotif = new Mock<IServicioNotificaciones>();
        var servicio = new ServicioPagos(
            new Mock<IRepositorioPagos>().Object, mockSubastas.Object,
            mockUsuarios.Object, new ServicioPasarelaPagos(), mockNotif.Object);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => servicio.ProcesarPagoAsync(new PagoCrearDTO { SubastaId = 1, UsuarioId = 5, Monto = 500m }));
        Assert.Contains("ganador", ex.Message.ToLower());
    }
}
