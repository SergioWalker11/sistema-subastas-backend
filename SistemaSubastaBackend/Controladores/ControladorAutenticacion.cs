using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/auth")]
public class ControladorAutenticacion : ControllerBase
{
    private readonly IServicioAutenticacion _servicioAutenticacion;

    public ControladorAutenticacion(IServicioAutenticacion servicioAutenticacion)
    {
        _servicioAutenticacion = servicioAutenticacion;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        try
        {
            var resultado = await _servicioAutenticacion.LoginAsync(dto);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(resultado, "Inicio de sesion exitoso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(AyudanteRespuestaAPI.RespuestaError(ex.Message, 401));
        }
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro([FromBody] RegistroDTO dto)
    {
        try
        {
            var resultado = await _servicioAutenticacion.RegistroAsync(dto);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(resultado, "Registro exitoso"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
    }
}
