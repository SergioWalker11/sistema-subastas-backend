using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Tests.Validacion;

public class MontoMinimo
{
    private readonly ValidadorPujas _validador = new();

    [Fact]
    public void MenorQue_PrecioInicial_RetornaError()
    {
        var s = new Subasta
        {
            Id = 1, PrecioInicial = 100m, PrecioActual = 100m, Estado = "activa",
            VendedorId = 5,
            FechaInicio = DateTime.UtcNow.AddDays(-1), FechaFin = DateTime.UtcNow.AddDays(5)
        };
        var e = _validador.ValidarPuja(50m, s, null, usuarioId: 1);
        Assert.Contains(e, m => m.Contains("minimo requerido es"));
    }

    [Fact]
    public void MenorQue_UltimaPujaMasIncremento_RetornaError()
    {
        var s = new Subasta
        {
            Id = 1, PrecioInicial = 100m, PrecioActual = 200m, Estado = "activa",
            VendedorId = 5,
            FechaInicio = DateTime.UtcNow.AddDays(-1), FechaFin = DateTime.UtcNow.AddDays(5)
        };
        var u = new Puja { Monto = 200m, FechaCreacion = DateTime.UtcNow };
        var e = _validador.ValidarPuja(200m, s, u, usuarioId: 1);
        Assert.Contains(e, m => m.Contains("minimo requerido es"));
    }

    [Fact]
    public void IgualA_UltimaPujaMasIncremento_EsValido()
    {
        var s = new Subasta
        {
            Id = 1, PrecioInicial = 100m, PrecioActual = 200m, Estado = "activa",
            VendedorId = 5,
            FechaInicio = DateTime.UtcNow.AddDays(-1), FechaFin = DateTime.UtcNow.AddDays(5)
        };
        var u = new Puja { Monto = 200m, FechaCreacion = DateTime.UtcNow };
        var e = _validador.ValidarPuja(201m, s, u, usuarioId: 1);
        Assert.DoesNotContain(e, m => m.Contains("minimo requerido es"));
    }
}
