using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Utilidades;

public class ValidadorPujas : IValidadorPujas
{
    private const decimal IncrementoMinimo = 1.00m;
    private const decimal MontoMaximo = 999999.99m;

    public List<string> ValidarPuja(decimal montoPuja, Subasta subasta, Puja? ultimaPuja, int usuarioId)
    {
        var montoMinimoRequerido = ultimaPuja != null
            ? ultimaPuja.Monto + IncrementoMinimo
            : subasta.PrecioInicial;

        var reglas = new (bool falla, string mensaje)[]
        {
            (montoPuja <= 0, "El monto de la puja debe ser mayor a cero"),
            (montoPuja > MontoMaximo, $"El monto maximo permitido es {MontoMaximo:C}"),
            (subasta.VendedorId == usuarioId, "No puedes pujar en tus propias subastas"),
            (subasta.Estado != "activa", $"No se puede pujar en una subasta en estado '{subasta.Estado}'"),
            (DateTime.UtcNow > subasta.FechaFin, "La subasta ha finalizado"),
            (DateTime.UtcNow < subasta.FechaInicio, "La subasta aun no ha comenzado"),
            (montoPuja < montoMinimoRequerido, $"El monto minimo requerido es {montoMinimoRequerido:C}"),
        };

        return reglas.Where(r => r.falla).Select(r => r.mensaje).ToList();
    }

    public decimal CalcularNuevoPrecio(decimal montoPuja, decimal precioActual)
    {
        return montoPuja > precioActual ? montoPuja : precioActual;
    }
}
