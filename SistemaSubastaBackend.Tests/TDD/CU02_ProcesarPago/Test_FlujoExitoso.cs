using Moq;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Pagos;

public class FlujoExitoso
{
    [Fact]
    public async Task Pago_Valido_RegistraYCambiaAVendida()
    {
        var subasta = new Subasta
        {
            Id = 1, Estado = "pendiente_pago", GanadorId = 5, VendedorId = 20,
            PrecioActual = 500m, FechaLimitePago = DateTime.UtcNow.AddHours(12),
            Producto = new Producto { Nombre = "Laptop" }
        };
        var mockSubastas = new Mock<IRepositorioSubastas>();
        mockSubastas.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(subasta);
        mockSubastas.Setup(r => r.ActualizarAsync(It.IsAny<Subasta>()))
            .Callback<Subasta>(s => subasta.Estado = s.Estado).ReturnsAsync(subasta);
        var mockUsuarios = new Mock<IRepositorioUsuarios>();
        mockUsuarios.Setup(r => r.ObtenerPorIdAsync(5)).ReturnsAsync(new Usuario
            { Id = 5, NombreCompleto = "C", Correo = "c@t.com", Rol = new Rol { Nombre = "comprador" } });
        var mockPagos = new Mock<IRepositorioPagos>();
        mockPagos.Setup(r => r.CrearAsync(It.IsAny<Pago>())).ReturnsAsync((Pago p) => p);
        var mockPasarela = new Mock<IServicioPasarelaPagos>();
        mockPasarela.Setup(p => p.ProcesarPagoAsync(500m, "C", "c@t.com"))
            .ReturnsAsync(new SistemaSubastaBackend.ServiciosExternos.ResultadoPasarela
            {
                CodigoTransaccion = "SUBASTA-TEST-1234", Aprobado = true, Mensaje = "Aprobado", Monto = 500m
            });
        var mockNotif = new Mock<IServicioNotificaciones>();
        mockNotif.Setup(n => n.NotificarPagoRecibidoAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        var servicio = new ServicioPagos(mockPagos.Object, mockSubastas.Object,
            mockUsuarios.Object, mockPasarela.Object, mockNotif.Object);

        var r = await servicio.ProcesarPagoAsync(new PagoCrearDTO { SubastaId = 1, UsuarioId = 5, Monto = 500m });

        Assert.Equal("aprobado", r.EstadoPago);
        Assert.NotNull(r.CodigoTransaccion);
        Assert.Equal("vendida", subasta.Estado);
        mockNotif.Verify(n => n.NotificarPagoRecibidoAsync(5, 20, "Laptop"), Times.Once);
    }
}
