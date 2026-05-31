using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Tests.Validacion;

public class MontoCero
{
    private readonly ValidadorPujas _validador = new();
    private readonly Subasta _subasta = new()
    {
        Id = 1, PrecioInicial = 100m, PrecioActual = 100m, Estado = "activa",
        VendedorId = 5,
        FechaInicio = DateTime.UtcNow.AddDays(-1), FechaFin = DateTime.UtcNow.AddDays(5)
    };

    [Fact]
    public void Monto_Cero_RetornaError()
    {
        var e = _validador.ValidarPuja(0m, _subasta, null, usuarioId: 1);
        Assert.Contains(e, m => m.Contains("mayor a cero"));
    }

    [Fact]
    public void Monto_Negativo_RetornaError()
    {
        var e = _validador.ValidarPuja(-50m, _subasta, null, usuarioId: 1);
        Assert.Contains(e, m => m.Contains("mayor a cero"));
    }
}
