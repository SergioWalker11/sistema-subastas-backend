using SistemaSubastaBackend.DTOs;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioNotificaciones
{
    Task<List<NotificacionRespuestaDTO>> ObtenerPorUsuarioAsync(int usuarioId);
    Task<NotificacionRespuestaDTO> CrearNotificacionAsync(NotificacionCrearDTO dto);
    Task<NotificacionRespuestaDTO> MarcarComoLeidaAsync(int id);
    Task<int> ContarNoLeidasAsync(int usuarioId);
    Task NotificarNuevaPujaAsync(int usuarioId, string titulo, string mensaje);
    Task NotificarSubastaGanadaAsync(int usuarioId, int subastaId);
    Task NotificarPagoRecibidoAsync(int compradorId, int vendedorId, string nombreProducto);
    Task NotificarIncumplimientoPagoAsync(int compradorId, int vendedorId, string nombreProducto);
    Task NotificarDepositoRealizadoAsync(int vendedorId, string banco);
}
