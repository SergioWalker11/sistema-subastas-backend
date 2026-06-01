using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Servicios;

public class ServicioNotificaciones : IServicioNotificaciones
{
    private readonly IRepositorioNotificaciones _repositorioNotificaciones;

    public ServicioNotificaciones(IRepositorioNotificaciones repositorioNotificaciones)
    {
        _repositorioNotificaciones = repositorioNotificaciones;
    }

    public async Task<List<NotificacionRespuestaDTO>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        var notificaciones = await _repositorioNotificaciones.ObtenerPorUsuarioAsync(usuarioId);
        return notificaciones.Select(MapearARespuestaDTO).ToList();
    }

    public async Task<NotificacionRespuestaDTO> CrearNotificacionAsync(NotificacionCrearDTO dto)
    {
        var notificacion = new Notificacion
        {
            UsuarioId = dto.UsuarioId,
            Titulo = dto.Titulo,
            Mensaje = dto.Mensaje,
            Leida = false,
            FechaCreacion = DateTime.UtcNow
        };

        notificacion = await _repositorioNotificaciones.CrearAsync(notificacion);
        return MapearARespuestaDTO(notificacion);
    }

    public async Task<NotificacionRespuestaDTO> MarcarComoLeidaAsync(int id)
    {
        var notificacion = await _repositorioNotificaciones.MarcarComoLeidaAsync(id);
        return MapearARespuestaDTO(notificacion);
    }

    public async Task<int> ContarNoLeidasAsync(int usuarioId)
    {
        return await _repositorioNotificaciones.ContarNoLeidasAsync(usuarioId);
    }

    public async Task NotificarNuevaPujaAsync(int usuarioId, string titulo, string mensaje)
    {
        await CrearNotificacionAsync(new NotificacionCrearDTO
        {
            UsuarioId = usuarioId,
            Titulo = titulo,
            Mensaje = mensaje
        });
    }

    public async Task NotificarSubastaGanadaAsync(int usuarioId, int subastaId)
    {
        await CrearNotificacionAsync(new NotificacionCrearDTO
        {
            UsuarioId = usuarioId,
            Titulo = "Subasta ganada",
            Mensaje = $"Has ganado la subasta #{subastaId}. Dispones de 24 horas para realizar el pago."
        });
    }

    public async Task NotificarPagoRecibidoAsync(int compradorId, int vendedorId, string nombreProducto)
    {
        await CrearNotificacionAsync(new NotificacionCrearDTO
        {
            UsuarioId = compradorId,
            Titulo = "Pago confirmado",
            Mensaje = $"Tu pago por '{nombreProducto}' fue registrado correctamente."
        });
        await CrearNotificacionAsync(new NotificacionCrearDTO
        {
            UsuarioId = vendedorId,
            Titulo = "Pago recibido",
            Mensaje = $"El comprador realizo el pago por '{nombreProducto}' exitosamente."
        });
    }

    public async Task NotificarIncumplimientoPagoAsync(int compradorId, int vendedorId, string nombreProducto)
    {
        await CrearNotificacionAsync(new NotificacionCrearDTO
        {
            UsuarioId = compradorId,
            Titulo = "Pago vencido",
            Mensaje = $"Perdiste la subasta de '{nombreProducto}' por incumplimiento de pago."
        });
        await CrearNotificacionAsync(new NotificacionCrearDTO
        {
            UsuarioId = vendedorId,
            Titulo = "Comprador incumplio",
            Mensaje = $"El comprador no realizo el pago de '{nombreProducto}' dentro del plazo establecido."
        });
    }

    public async Task NotificarDepositoRealizadoAsync(int vendedorId, string banco)
    {
        await CrearNotificacionAsync(new NotificacionCrearDTO
        {
            UsuarioId = vendedorId,
            Titulo = "Deposito realizado",
            Mensaje = $"El dinero fue transferido a tu cuenta bancaria en {banco}. El dinero ya esta disponible."
        });
    }

    private NotificacionRespuestaDTO MapearARespuestaDTO(Notificacion notificacion)
    {
        return new NotificacionRespuestaDTO
        {
            Id = notificacion.Id,
            UsuarioId = notificacion.UsuarioId,
            Titulo = notificacion.Titulo,
            Mensaje = notificacion.Mensaje,
            Leida = notificacion.Leida,
            FechaCreacion = notificacion.FechaCreacion
        };
    }
}
