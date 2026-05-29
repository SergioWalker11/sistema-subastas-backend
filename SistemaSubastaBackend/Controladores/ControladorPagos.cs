using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/pagos")]
public class ControladorPagos : ControllerBase
{
    private readonly IServicioPagos _servicioPagos;

    public ControladorPagos(IServicioPagos servicioPagos)
    {
        _servicioPagos = servicioPagos;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProcesarPago([FromBody] PagoCrearDTO dto)
    {
        try
        {
            var pago = await _servicioPagos.ProcesarPagoAsync(dto);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(pago, "Pago procesado exitosamente"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(AyudanteRespuestaAPI.RespuestaError(ex.Message, 404));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPago(int id)
    {
        var pago = await _servicioPagos.ObtenerPagoAsync(id);
        if (pago == null)
        {
            return NotFound(AyudanteRespuestaAPI.RespuestaError($"No se encontro el pago con ID {id}", 404));
        }
        return Ok(AyudanteRespuestaAPI.RespuestaExito(pago));
    }

    [HttpGet("usuario/{usuarioId}")]
    [Authorize]
    public async Task<IActionResult> ObtenerPagosUsuario(int usuarioId)
    {
        var pagos = await _servicioPagos.ObtenerPagosUsuarioAsync(usuarioId);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(pagos));
    }
}
