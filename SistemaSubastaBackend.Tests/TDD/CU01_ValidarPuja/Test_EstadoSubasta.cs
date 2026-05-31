using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Tests.Validacion;

public class EstadoSubasta
{
    private readonly ValidadorPujas _validador = new();

    [Theory]
    [InlineData("pendiente_pago")]
    [InlineData("vendida")]
    [InlineData("incumplida")]
    [InlineData("cancelada")]
    [InlineData("finalizada")]
    public void Estado_NoActiva_RetornaError(string estado)
    {
        var s = new Subasta
        {
            Id = 1, PrecioInicial = 100m, PrecioActual = 100m, Estado = estado,
            VendedorId = 5,
            FechaInicio = DateTime.UtcNow.AddDays(-1), FechaFin = DateTime.UtcNow.AddDays(5)
        };
        var e = _validador.ValidarPuja(200m, s, null, usuarioId: 1);
        Assert.Contains(e, m => m.Contains("activa") || m.Contains(estado));
    }
}
