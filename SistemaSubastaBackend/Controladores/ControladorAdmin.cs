using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "administrador")]
public class ControladorAdmin : ControllerBase
{
    private readonly IServicioAdmin _servicioAdmin;

    public ControladorAdmin(IServicioAdmin servicioAdmin)
    {
        _servicioAdmin = servicioAdmin;
    }

    [HttpGet("usuarios")]
    public async Task<IActionResult> ListarUsuarios()
    {
        var usuarios = await _servicioAdmin.ListarUsuariosAsync();
        return Ok(AyudanteRespuestaAPI.RespuestaExito(usuarios));
    }

    [HttpPatch("usuarios/{usuarioId}/rol")]
    public async Task<IActionResult> CambiarRol(int usuarioId, [FromBody] int rolId)
    {
        try
        {
            await _servicioAdmin.CambiarRolUsuarioAsync(usuarioId, rolId);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(new { }, "Rol actualizado exitosamente"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(AyudanteRespuestaAPI.RespuestaError(ex.Message, 404));
        }
    }

    [HttpPatch("subastas/{subastaId}/cancelar")]
    public async Task<IActionResult> CancelarSubasta(int subastaId)
    {
        try
        {
            await _servicioAdmin.CancelarSubastaAsync(subastaId);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(new { }, "Subasta cancelada exitosamente"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(AyudanteRespuestaAPI.RespuestaError(ex.Message, 404));
        }
    }
}
