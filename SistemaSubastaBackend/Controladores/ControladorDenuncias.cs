using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;
using System.Security.Claims;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/denuncias")]
public class ControladorDenuncias : ControllerBase
{
    private readonly IRepositorioDenuncias _repoDenuncias;
    private readonly IRepositorioUsuarios _repoUsuarios;

    public ControladorDenuncias(IRepositorioDenuncias repoDenuncias, IRepositorioUsuarios repoUsuarios)
    {
        _repoDenuncias = repoDenuncias;
        _repoUsuarios = repoUsuarios;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Crear([FromBody] DenunciaCrearDTO dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Motivo))
                return BadRequest(AyudanteRespuestaAPI.RespuestaError("El motivo es obligatorio"));

            var denunciado = await _repoUsuarios.ObtenerPorIdAsync(dto.DenunciadoId);
            if (denunciado == null)
                return NotFound(AyudanteRespuestaAPI.RespuestaError("Usuario denunciado no encontrado", 404));

            var denuncianteId = ObtenerUsuarioId();
            if (denuncianteId == 0)
                return Unauthorized(AyudanteRespuestaAPI.RespuestaError("No se pudo identificar al usuario", 401));

            if (denuncianteId == dto.DenunciadoId)
                return BadRequest(AyudanteRespuestaAPI.RespuestaError("No puedes denunciarte a ti mismo"));

            var denuncia = new Denuncia
            {
                DenuncianteId = denuncianteId,
                DenunciadoId = dto.DenunciadoId,
                Motivo = dto.Motivo,
                Estado = "pendiente",
                FechaCreacion = DateTime.UtcNow,
                Denunciante = null!,
                Denunciado = null!
            };

            denuncia = await _repoDenuncias.CrearAsync(denuncia);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(new { denuncia.Id, denuncia.Estado }, "Denuncia registrada. Un administrador la revisara."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, AyudanteRespuestaAPI.RespuestaError($"Error interno: {ex.Message}"));
        }
    }

    [HttpGet]
    [Authorize(Roles = "administrador")]
    public async Task<IActionResult> Listar()
    {
        var denuncias = await _repoDenuncias.ObtenerTodasAsync();
        var resultado = denuncias.Select(d => new
        {
            d.Id,
            Denunciante = d.Denunciante.NombreCompleto,
            DenuncianteId = d.DenuncianteId,
            Denunciado = d.Denunciado.NombreCompleto,
            DenunciadoId = d.DenunciadoId,
            d.Motivo,
            d.Estado,
            d.FechaCreacion
        });
        return Ok(AyudanteRespuestaAPI.RespuestaExito(resultado));
    }

    [HttpPatch("{id}/resolver")]
    [Authorize(Roles = "administrador")]
    public async Task<IActionResult> Resolver(int id, [FromBody] string accion)
    {
        var denuncia = await _repoDenuncias.ObtenerPorIdAsync(id);
        if (denuncia == null)
            return NotFound(AyudanteRespuestaAPI.RespuestaError("Denuncia no encontrada", 404));

        denuncia.Estado = accion == "suspender" ? "resuelta" : "rechazada";
        await _repoDenuncias.ActualizarAsync(denuncia);

        if (accion == "suspender")
        {
            var denunciado = await _repoUsuarios.ObtenerPorIdAsync(denuncia.DenunciadoId);
            if (denunciado != null)
            {
                denunciado.EstaSuspendido = true;
                await _repoUsuarios.ActualizarAsync(denunciado);
            }
        }

        return Ok(AyudanteRespuestaAPI.RespuestaExito(new { }, accion == "suspender" ? "Usuario suspendido" : "Denuncia rechazada"));
    }

    private int ObtenerUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }
}
