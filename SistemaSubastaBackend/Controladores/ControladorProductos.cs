using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/productos")]
public class ControladorProductos : ControllerBase
{
    private readonly IServicioProductos _servicioProductos;

    public ControladorProductos(IServicioProductos servicioProductos)
    {
        _servicioProductos = servicioProductos;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var dtos = await _servicioProductos.ListarAsync();
        return Ok(AyudanteRespuestaAPI.RespuestaExito(dtos));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Obtener(int id)
    {
        var dto = await _servicioProductos.ObtenerAsync(id);
        if (dto == null) return NotFound(AyudanteRespuestaAPI.RespuestaError("Producto no encontrado", 404));
        return Ok(AyudanteRespuestaAPI.RespuestaExito(dto));
    }

    [HttpPost]
    [Authorize(Roles = "vendedor,administrador")]
    public async Task<IActionResult> Crear([FromBody] ProductoCrearDTO dto)
    {
        var producto = await _servicioProductos.CrearAsync(dto);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(producto, "Producto creado exitosamente"));
    }

    [HttpGet("categorias")]
    public async Task<IActionResult> ListarCategorias()
    {
        var categorias = await _servicioProductos.ListarCategoriasAsync();
        return Ok(AyudanteRespuestaAPI.RespuestaExito(categorias));
    }
}
