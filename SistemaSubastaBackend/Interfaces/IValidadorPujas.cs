using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IValidadorPujas
{
    List<string> ValidarPuja(decimal montoPuja, Subasta subasta, Puja? ultimaPuja, int usuarioId);
    decimal CalcularNuevoPrecio(decimal montoPuja, decimal precioActual);
}
