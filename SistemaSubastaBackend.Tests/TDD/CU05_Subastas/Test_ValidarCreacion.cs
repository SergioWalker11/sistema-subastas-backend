using Moq;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Servicios;

namespace SistemaSubastaBackend.Tests.Subastas;

public class ValidarCreacion
{
    [Theory]
    [InlineData("", "", 0, "obligatorio")]
    [InlineData("test", "", 0, "obligatoria")]
    [InlineData("test", "desc", 0, "obligatoria")]
    [InlineData("test", "desc", 1, "mayor a cero")]
    public async Task Datos_Invalidos_LanzaArgumentException(string nombre, string descripcion, int categoriaId, string palabra)
    {
        var servicio = new ServicioSubastas(new Mock<IRepositorioSubastas>().Object,
            new Mock<IRepositorioPujas>().Object, new Mock<IRepositorioPagos>().Object);
        var dto = new SubastaCrearDTO
        {
            NombreProducto = nombre, DescripcionProducto = descripcion, CategoriaId = categoriaId,
            PrecioInicial = 0, VendedorId = 5,
            FechaInicio = DateTime.UtcNow.AddDays(1), FechaFin = DateTime.UtcNow.AddDays(5)
        };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => servicio.CrearSubastaAsync(dto));
        Assert.Contains(palabra, ex.Message.ToLower());
    }
}
