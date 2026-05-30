using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Utilidades;

public class ValidadorPujas
{
    private static readonly HashSet<string> EstadosNoPermitidos = new()
    {
        "finalizada", "pendiente_pago", "vendida", "incumplida", "cancelada"
    };

    private const decimal IncrementoMinimo = 1.00m;
    private const decimal MontoMaximo = 999999.99m;

    public List<string> ValidarPuja(decimal montoPuja, Subasta subasta, Puja? ultimaPuja, int usuarioId)
    {
        var errores = new List<string>();

        if (montoPuja <= 0)
            errores.Add("El monto de la puja debe ser mayor a cero");

        if (montoPuja > MontoMaximo)
            errores.Add($"El monto maximo permitido es {MontoMaximo:C}");

        if (subasta.VendedorId == usuarioId)
            errores.Add("No puedes pujar en tus propias subastas");

        if (subasta.Estado != "activa")
            errores.Add("La subasta no se encuentra activa");

        if (EstadosNoPermitidos.Contains(subasta.Estado))
            errores.Add($"No se puede pujar en una subasta en estado '{subasta.Estado}'");

        if (DateTime.UtcNow > subasta.FechaFin)
            errores.Add("La subasta ha finalizado");

        if (DateTime.UtcNow < subasta.FechaInicio)
            errores.Add("La subasta aun no ha comenzado");

        var montoMinimoRequerido = ultimaPuja != null
            ? ultimaPuja.Monto + IncrementoMinimo
            : subasta.PrecioInicial;

        if (montoPuja < montoMinimoRequerido)
            errores.Add($"El monto minimo requerido es {montoMinimoRequerido:C}");

        return errores;
    }

    public decimal CalcularNuevoPrecio(decimal montoPuja, decimal precioActual)
    {
        return montoPuja > precioActual ? montoPuja : precioActual;
    }
}
