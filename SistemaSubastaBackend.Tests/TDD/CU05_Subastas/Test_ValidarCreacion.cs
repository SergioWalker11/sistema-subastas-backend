using Moq;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Subastas;

public class ValidarCreacion
{
    [Theory]
    [InlineData("mayor a cero")]
    [InlineData("obligatoria")]
    public async Task Datos_Invalidos_LanzaArgumentException(string palabra)
    {
        var servicio = new ServicioSubastas(new Mock<IRepositorioSubastas>().Object,
            new Mock<IRepositorioPujas>().Object, new Mock<IRepositorioPagos>().Object);
        var dto = new SubastaCrearDTO
        {
            NombreProducto = "test",
            DescripcionProducto = "desc",
            CategoriaId = 1,
            ProductoId = 1,
            PrecioInicial = 0,
            VendedorId = 5,
            FechaInicio = default,
            FechaFin = DateTime.UtcNow.AddDays(5)
        };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => servicio.CrearSubastaAsync(dto));
        Assert.Contains(palabra, ex.Message.ToLower());
    }

    [Fact]
    public async Task Datos_Validos_CreaSubastaCorrectamente()
    {
        var repoSubastas = new Mock<IRepositorioSubastas>();
        repoSubastas.Setup(r => r.CrearAsync(It.IsAny<Subasta>())).ReturnsAsync((Subasta s) => { s.Id = 1; return s; });
        var servicio = new ServicioSubastas(repoSubastas.Object,
            new Mock<IRepositorioPujas>().Object, new Mock<IRepositorioPagos>().Object);
        var dto = new SubastaCrearDTO
        {
            NombreProducto = "test",
            DescripcionProducto = "desc",
            CategoriaId = 1,
            ProductoId = 1,
            PrecioInicial = 100,
            VendedorId = 5,
            FechaInicio = DateTime.UtcNow.AddDays(1),
            FechaFin = DateTime.UtcNow.AddDays(5)
        };

        var resultado = await servicio.CrearSubastaAsync(dto);
        Assert.NotNull(resultado);
        Assert.Equal("activa", resultado.Estado);
        Assert.Equal(100, resultado.PrecioActual);
    }
}
