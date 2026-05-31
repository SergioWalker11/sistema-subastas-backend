using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Servicios;

public class ServicioProductos : IServicioProductos
{
    private readonly IRepositorioProductos _repositorio;
    private readonly IRepositorioCategorias _repositorioCategorias;

    public ServicioProductos(IRepositorioProductos repositorio, IRepositorioCategorias repositorioCategorias)
    {
        _repositorio = repositorio;
        _repositorioCategorias = repositorioCategorias;
    }

    public async Task<List<ProductoDTO>> ListarAsync()
    {
        var productos = await _repositorio.ObtenerTodosAsync();
        return productos.Select(MapearADTO).ToList();
    }

    public async Task<ProductoDTO?> ObtenerAsync(int id)
    {
        var producto = await _repositorio.ObtenerPorIdAsync(id);
        return producto == null ? null : MapearADTO(producto);
    }

    public async Task<ProductoDTO> CrearAsync(ProductoCrearDTO dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            CategoriaId = dto.CategoriaId
        };

        producto = await _repositorio.CrearAsync(producto);
        return MapearADTO(producto);
    }

    public async Task<List<Categoria>> ListarCategoriasAsync()
    {
        return await _repositorioCategorias.ObtenerTodasAsync();
    }

    private static ProductoDTO MapearADTO(Producto p) => new()
    {
        Id = p.Id,
        Nombre = p.Nombre,
        Descripcion = p.Descripcion,
        CategoriaId = p.CategoriaId,
        CategoriaNombre = p.Categoria?.Nombre ?? string.Empty
    };
}
