using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IRepositorioNotificaciones
{
    Task<List<Notificacion>> ObtenerPorUsuarioAsync(int usuarioId);
    Task<Notificacion> CrearAsync(Notificacion notificacion);
    Task<Notificacion> MarcarComoLeidaAsync(int id);
    Task<int> ContarNoLeidasAsync(int usuarioId);
}
