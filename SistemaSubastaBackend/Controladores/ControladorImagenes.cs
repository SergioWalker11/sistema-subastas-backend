using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/imagenes")]
public class ControladorImagenes : ControllerBase
{
    private readonly IServicioImagenes _servicioImagenes;

    public ControladorImagenes(IServicioImagenes servicioImagenes)
    {
        _servicioImagenes = servicioImagenes;
    }

    [HttpPost("{productoId}")]
    [Authorize(Roles = "vendedor,administrador")]
    public async Task<IActionResult> Subir(int productoId, IFormFile archivo, [FromQuery] bool esPrincipal = false)
    {
        try
        {
            var imagen = await _servicioImagenes.SubirImagenAsync(productoId, archivo, esPrincipal);
            return Ok(AyudanteRespuestaAPI.RespuestaExito(imagen, "Imagen subida exitosamente"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(AyudanteRespuestaAPI.RespuestaError(ex.Message));
        }
    }

    [HttpGet("producto/{productoId}")]
    public async Task<IActionResult> ObtenerPorProducto(int productoId)
    {
        var imagenes = await _servicioImagenes.ObtenerPorProductoAsync(productoId);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(imagenes));
    }
}
