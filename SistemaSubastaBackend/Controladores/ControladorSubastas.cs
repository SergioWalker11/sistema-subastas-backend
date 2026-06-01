using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/subastas")]
public class ControladorSubastas : ControllerBase
{
    private readonly IServicioSubastas _servicioSubastas;

    public ControladorSubastas(IServicioSubastas servicioSubastas)
    {
        _servicioSubastas = servicioSubastas;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var subastas = await _servicioSubastas.ListarSubastasAsync();
        return Ok(AyudanteRespuestaAPI.RespuestaExito(subastas));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerDetalle(int id)
    {
        var subasta = await _servicioSubastas.ObtenerDetalleAsync(id);
        if (subasta == null)
        {
            return NotFound(AyudanteRespuestaAPI.RespuestaError($"No se encontro la subasta con ID {id}", 404));
        }
        return Ok(AyudanteRespuestaAPI.RespuestaExito(subasta));
    }

    [HttpPost]
    [Authorize(Roles = "vendedor,administrador")]
    public async Task<IActionResult> Crear([FromBody] SubastaCrearDTO dto)
    {
        try
        {
            var subasta = await _servicioSubastas.CrearSubastaAsync(dto);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(subasta, "Subasta creada exitosamente"));
        }
        catch (Exception ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
    }

    [HttpPatch("{id}/estado")]
    [Authorize(Roles = "vendedor,administrador")]
    public async Task<IActionResult> ActualizarEstado(int id, [FromBody] string estado)
    {
        try
        {
            var subasta = await _servicioSubastas.ActualizarEstadoAsync(id, estado);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(subasta, "Estado actualizado exitosamente"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(AyudanteRespuestaAPI.RespuestaError(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "vendedor,administrador")]
    public async Task<IActionResult> Editar(int id, [FromBody] SubastaEditarDTO dto)
    {
        try
        {
            var subasta = await _servicioSubastas.EditarSubastaAsync(id, dto);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(subasta, "Subasta editada exitosamente"));
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

    [HttpPatch("{id}/cancelar")]
    [Authorize(Roles = "vendedor,administrador")]
    public async Task<IActionResult> Cancelar(int id, [FromQuery] int vendedorId)
    {
        try
        {
            var subasta = await _servicioSubastas.CancelarSubastaAsync(id, vendedorId);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(subasta, "Subasta cancelada exitosamente"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(AyudanteRespuestaAPI.RespuestaError(ex.Message, 404));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
    }

    [HttpGet("vendedor/{vendedorId}")]
    [Authorize]
    public async Task<IActionResult> ListarPorVendedor(int vendedorId)
    {
        var subastas = await _servicioSubastas.ListarPorVendedorAsync(vendedorId);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(subastas));
    }

    [HttpGet("ganadas/{usuarioId}")]
    [Authorize]
    public async Task<IActionResult> ListarGanadas(int usuarioId)
    {
        var ganadas = await _servicioSubastas.ListarGanadasPorUsuarioAsync(usuarioId);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(ganadas));
    }

    [HttpGet("pendientes-pago/{usuarioId}")]
    [Authorize]
    public async Task<IActionResult> ListarPendientesPago(int usuarioId)
    {
        var pendientes = await _servicioSubastas.ListarPendientesPagoAsync(usuarioId);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(pendientes));
    }

    [HttpGet("ventas/{vendedorId}")]
    [Authorize(Roles = "vendedor,administrador")]
    public async Task<IActionResult> ListarVentas(int vendedorId)
    {
        var ventas = await _servicioSubastas.ListarVentasAsync(vendedorId);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(ventas));
    }
}
