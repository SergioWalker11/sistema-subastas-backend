using Moq;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Subastas;

public class CrearSubasta
{
    [Fact]
    public async Task Datos_Validos_CreaSubastaEnEstadoActiva()
    {
        var mockRepo = new Mock<IRepositorioSubastas>();
        mockRepo.Setup(r => r.CrearAsync(It.IsAny<Subasta>())).ReturnsAsync((Subasta s) => { s.Id = 1; return s; });

        var servicio = new ServicioSubastas(mockRepo.Object,
            new Mock<IRepositorioPujas>().Object, new Mock<IRepositorioPagos>().Object);
        var dto = new SubastaCrearDTO
        {
            ProductoId = 1, VendedorId = 5, PrecioInicial = 100m,
            FechaInicio = DateTime.UtcNow.AddDays(1), FechaFin = DateTime.UtcNow.AddDays(5),
            NombreProducto = "Laptop", DescripcionProducto = "Gamer", CategoriaId = 1
        };

        var result = await servicio.CrearSubastaAsync(dto);

        Assert.Equal("activa", result.Estado);
        Assert.Equal(100m, result.PrecioInicial);
    }
}
