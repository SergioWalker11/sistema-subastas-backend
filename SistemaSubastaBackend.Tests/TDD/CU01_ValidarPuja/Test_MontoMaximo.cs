using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Tests.Validacion;

public class MontoMaximo
{
    private readonly ValidadorPujas _validador = new();
    private readonly Subasta _subasta = new()
    {
        Id = 1, PrecioInicial = 100m, PrecioActual = 100m, Estado = "activa",
        VendedorId = 5,
        FechaInicio = DateTime.UtcNow.AddDays(-1), FechaFin = DateTime.UtcNow.AddDays(5)
    };

    [Fact]
    public void Monto_Excede_999999_RetornaError()
    {
        var e = _validador.ValidarPuja(2000000m, _subasta, null, usuarioId: 1);
        Assert.Contains(e, m => m.Contains("maximo permitido"));
    }

    [Fact]
    public void Monto_DentroDelLimite_NoRetornaErrorDeMaximo()
    {
        var e = _validador.ValidarPuja(500m, _subasta, null, usuarioId: 1);
        Assert.DoesNotContain(e, m => m.Contains("maximo permitido"));
    }
}
