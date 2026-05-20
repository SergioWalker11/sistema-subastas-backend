using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/notificaciones")]
public class ControladorNotificaciones : ControllerBase
{
    private readonly IServicioNotificaciones _servicioNotificaciones;

    public ControladorNotificaciones(IServicioNotificaciones servicioNotificaciones)
    {
        _servicioNotificaciones = servicioNotificaciones;
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> ObtenerPorUsuario(int usuarioId)
    {
        var notificaciones = await _servicioNotificaciones.ObtenerPorUsuarioAsync(usuarioId);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(notificaciones));
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] NotificacionCrearDTO dto)
    {
        try
        {
            var notificacion = await _servicioNotificaciones.CrearNotificacionAsync(dto);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(notificacion, "Notificacion creada exitosamente"));
        }
        catch (Exception ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
    }

    [HttpPatch("{id}/leida")]
    public async Task<IActionResult> MarcarComoLeida(int id)
    {
        try
        {
            var notificacion = await _servicioNotificaciones.MarcarComoLeidaAsync(id);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(notificacion, "Notificacion marcada como leida"));
        }
        catch (Exception ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
    }

    [HttpGet("usuario/{usuarioId}/no-leidas")]
    public async Task<IActionResult> ContarNoLeidas(int usuarioId)
    {
        var cantidad = await _servicioNotificaciones.ContarNoLeidasAsync(usuarioId);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(new { cantidad }));
    }
}
