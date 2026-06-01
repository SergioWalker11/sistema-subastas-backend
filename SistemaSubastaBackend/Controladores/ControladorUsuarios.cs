using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/usuarios")]
public class ControladorUsuarios : ControllerBase
{
    private readonly IRepositorioUsuarios _repoUsuarios;
    private readonly IRepositorioDatosBancarios _repoDatosBancarios;

    public ControladorUsuarios(IRepositorioUsuarios repoUsuarios, IRepositorioDatosBancarios repoDatosBancarios)
    {
        _repoUsuarios = repoUsuarios;
        _repoDatosBancarios = repoDatosBancarios;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerPerfil(int id)
    {
        var usuario = await _repoUsuarios.ObtenerPorIdAsync(id);
        if (usuario == null)
            return NotFound(AyudanteRespuestaAPI.RespuestaError("Usuario no encontrado", 404));

        var datos = await _repoDatosBancarios.ObtenerPorUsuarioAsync(id);

        return Ok(AyudanteRespuestaAPI.RespuestaExito(new PerfilDTO
        {
            Id = usuario.Id,
            NombreCompleto = usuario.NombreCompleto,
            Correo = usuario.Correo,
            Rol = usuario.Rol?.Nombre ?? "",
            EstaSuspendido = usuario.EstaSuspendido,
            DatosBancarios = datos == null ? null : new DatosBancariosDTO
            {
                Banco = datos.Banco,
                TipoCuenta = datos.TipoCuenta,
                NumeroCuenta = datos.NumeroCuenta,
                Titular = datos.Titular
            }
        }));
    }

    [HttpPut("{id}/banco")]
    [Authorize(Roles = "vendedor")]
    public async Task<IActionResult> GuardarDatosBancarios(int id, [FromBody] DatosBancariosDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Banco))
            return BadRequest(AyudanteRespuestaAPI.RespuestaError("El banco es obligatorio"));
        if (string.IsNullOrWhiteSpace(dto.NumeroCuenta))
            return BadRequest(AyudanteRespuestaAPI.RespuestaError("El numero de cuenta es obligatorio"));
        if (string.IsNullOrWhiteSpace(dto.Titular))
            return BadRequest(AyudanteRespuestaAPI.RespuestaError("El titular es obligatorio"));

        var datos = new DatosBancarios
        {
            UsuarioId = id,
            Banco = dto.Banco,
            TipoCuenta = dto.TipoCuenta,
            NumeroCuenta = dto.NumeroCuenta,
            Titular = dto.Titular
        };

        await _repoDatosBancarios.GuardarAsync(datos);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(dto, "Datos bancarios guardados"));
    }
}
