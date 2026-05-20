using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/pujas")]
public class ControladorPujas : ControllerBase
{
    private readonly IServicioPujas _servicioPujas;

    public ControladorPujas(IServicioPujas servicioPujas)
    {
        _servicioPujas = servicioPujas;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarPuja([FromBody] PujaCrearDTO dto)
    {
        try
        {
            var puja = await _servicioPujas.RegistrarPujaAsync(dto);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(puja, "Puja registrada exitosamente"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(AyudanteRespuestaAPI.RespuestaError(ex.Message, 404));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
    }

    [HttpGet("subasta/{subastaId}")]
    public async Task<IActionResult> ObtenerHistorial(int subastaId)
    {
        var historial = await _servicioPujas.ObtenerHistorialAsync(subastaId);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(historial));
    }
}
