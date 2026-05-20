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
}
