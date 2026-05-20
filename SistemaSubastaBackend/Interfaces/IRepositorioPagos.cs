using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IRepositorioPagos
{
    Task<Pago> CrearAsync(Pago pago);
    Task<Pago?> ObtenerPorIdAsync(int id);
    Task<List<Pago>> ObtenerPorUsuarioAsync(int usuarioId);
    Task<Pago> ActualizarAsync(Pago pago);
}
