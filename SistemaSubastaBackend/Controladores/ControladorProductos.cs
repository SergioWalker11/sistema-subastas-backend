using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Controladores;

[ApiController]
[Route("api/productos")]
public class ControladorProductos : ControllerBase
{
    private readonly IRepositorioProductos _repositorio;
    private readonly IRepositorioCategorias _repositorioCategorias;

    public ControladorProductos(IRepositorioProductos repositorio, IRepositorioCategorias repositorioCategorias)
    {
        _repositorio = repositorio;
        _repositorioCategorias = repositorioCategorias;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var productos = await _repositorio.ObtenerTodosAsync();
        var dtos = productos.Select(p => new ProductoDTO
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre ?? string.Empty
        }).ToList();
        return Ok(AyudanteRespuestaAPI.RespuestaExito(dtos));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Obtener(int id)
    {
        var p = await _repositorio.ObtenerPorIdAsync(id);
        if (p == null) return NotFound(AyudanteRespuestaAPI.RespuestaError("Producto no encontrado", 404));

        return Ok(AyudanteRespuestaAPI.RespuestaExito(new ProductoDTO
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre ?? string.Empty
        }));
    }

    [HttpPost]
    [Authorize(Roles = "vendedor,administrador")]
    public async Task<IActionResult> Crear([FromBody] ProductoCrearDTO dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            CategoriaId = dto.CategoriaId
        };

        producto = await _repositorio.CrearAsync(producto);
        return Ok(AyudanteRespuestaAPI.RespuestaExito(new ProductoDTO
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            CategoriaId = producto.CategoriaId
        }, "Producto creado exitosamente"));
    }

    [HttpGet("categorias")]
    public async Task<IActionResult> ListarCategorias()
    {
        var categorias = await _repositorioCategorias.ObtenerTodasAsync();
        return Ok(AyudanteRespuestaAPI.RespuestaExito(categorias));
    }
}
