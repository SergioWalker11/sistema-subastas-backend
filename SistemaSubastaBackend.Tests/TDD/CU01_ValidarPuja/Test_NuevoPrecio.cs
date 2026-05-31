using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Tests.Validacion;

public class NuevoPrecio
{
    private readonly ValidadorPujas _validador = new();

    [Fact]
    public void Puja_MayorQuePrecioActual_RetornaMontoPuja()
    {
        Assert.Equal(500m, _validador.CalcularNuevoPrecio(500m, 300m));
    }

    [Fact]
    public void Puja_MenorQuePrecioActual_MantienePrecio()
    {
        Assert.Equal(300m, _validador.CalcularNuevoPrecio(200m, 300m));
    }
}
