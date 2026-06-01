using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "administrador")]
public class ControladorAdmin : ControllerBase
{
    private readonly IServicioAdmin _servicioAdmin;
    private readonly IRepositorioCategorias _repoCategorias;

    public ControladorAdmin(IServicioAdmin servicioAdmin, IRepositorioCategorias repoCategorias)
    {
        _servicioAdmin = servicioAdmin;
        _repoCategorias = repoCategorias;
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

    [HttpPatch("usuarios/{usuarioId}/suspender")]
    public async Task<IActionResult> SuspenderUsuario(int usuarioId, [FromBody] bool suspender)
    {
        var usuario = await _servicioAdmin.ObtenerUsuarioAsync(usuarioId);
        if (usuario == null)
            return NotFound(AyudanteRespuestaAPI.RespuestaError("Usuario no encontrado", 404));

        usuario.EstaSuspendido = suspender;
        await _servicioAdmin.ActualizarUsuarioAsync(usuario);

        var mensaje = suspender ? "Usuario suspendido exitosamente" : "Usuario desbaneado exitosamente";
        return Ok(AyudanteRespuestaAPI.RespuestaExito(new { }, mensaje));
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

    [HttpPost("categorias")]
    public async Task<IActionResult> CrearCategoria([FromBody] CrearCategoriaDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest(AyudanteRespuestaAPI.RespuestaError("El nombre es obligatorio"));

        var categoria = new Categoria { Nombre = dto.Nombre, Descripcion = dto.Descripcion ?? string.Empty };
        var creada = await _repoCategorias.CrearAsync(categoria);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(creada, "Categoría creada exitosamente"));
    }

    [HttpPut("categorias/{id}")]
    public async Task<IActionResult> EditarCategoria(int id, [FromBody] CrearCategoriaDTO dto)
    {
        var categoria = await _repoCategorias.ObtenerPorIdAsync(id);
        if (categoria == null)
            return NotFound(AyudanteRespuestaAPI.RespuestaError("Categoría no encontrada", 404));

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest(AyudanteRespuestaAPI.RespuestaError("El nombre es obligatorio"));

        categoria.Nombre = dto.Nombre;
        categoria.Descripcion = dto.Descripcion ?? string.Empty;
        await _repoCategorias.ActualizarAsync(categoria);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(categoria, "Categoría actualizada exitosamente"));
    }
}

public class CrearCategoriaDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
