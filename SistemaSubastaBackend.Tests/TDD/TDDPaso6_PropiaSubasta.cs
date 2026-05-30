using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Tests.Validacion;

public class PropiaSubasta
{
    private readonly ValidadorPujas _validador = new();

    [Fact]
    public void Vendedor_PujaSuPropiaSubasta_RetornaError()
    {
        var s = new Subasta
        {
            Id = 1, PrecioInicial = 100m, PrecioActual = 100m, Estado = "activa",
            VendedorId = 5,
            FechaInicio = DateTime.UtcNow.AddDays(-1), FechaFin = DateTime.UtcNow.AddDays(5)
        };
        var e = _validador.ValidarPuja(200m, s, null, usuarioId: 5);
        Assert.Contains(e, m => m.Contains("propias subastas"));
    }

    [Fact]
    public void Comprador_PujaSubastaAjena_EsValido()
    {
        var s = new Subasta
        {
            Id = 1, PrecioInicial = 100m, PrecioActual = 100m, Estado = "activa",
            VendedorId = 5,
            FechaInicio = DateTime.UtcNow.AddDays(-1), FechaFin = DateTime.UtcNow.AddDays(5)
        };
        var e = _validador.ValidarPuja(200m, s, null, usuarioId: 3);
        Assert.DoesNotContain(e, m => m.Contains("propias subastas"));
    }
}
