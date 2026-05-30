using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Tests.Validacion;

public class Fechas
{
    private readonly ValidadorPujas _validador = new();

    [Fact]
    public void FechaFin_Vencida_RetornaError()
    {
        var s = new Subasta
        {
            Id = 1, PrecioInicial = 100m, PrecioActual = 100m, Estado = "activa",
            VendedorId = 5,
            FechaInicio = DateTime.UtcNow.AddDays(-5), FechaFin = DateTime.UtcNow.AddDays(-1)
        };
        var e = _validador.ValidarPuja(200m, s, null, usuarioId: 1);
        Assert.Contains(e, m => m.Contains("finalizado"));
    }

    [Fact]
    public void FechaInicio_NoComenzada_RetornaError()
    {
        var s = new Subasta
        {
            Id = 1, PrecioInicial = 100m, PrecioActual = 100m, Estado = "activa",
            VendedorId = 5,
            FechaInicio = DateTime.UtcNow.AddDays(2), FechaFin = DateTime.UtcNow.AddDays(5)
        };
        var e = _validador.ValidarPuja(200m, s, null, usuarioId: 1);
        Assert.Contains(e, m => m.Contains("comenzado"));
    }
}
