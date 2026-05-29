using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioProductos : IRepositorioProductos
{
    private readonly ContextoSubastas _contexto;

    public RepositorioProductos(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Producto>> ObtenerTodosAsync()
    {
        return await _contexto.Productos
            .Include(p => p.Categoria)
            .ToListAsync();
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Producto> CrearAsync(Producto producto)
    {
        _contexto.Productos.Add(producto);
        await _contexto.SaveChangesAsync();
        return producto;
    }

    public async Task<Producto> ActualizarAsync(Producto producto)
    {
        _contexto.Productos.Update(producto);
        await _contexto.SaveChangesAsync();
        return producto;
    }

    public async Task EliminarAsync(int id)
    {
        var producto = await _contexto.Productos.FindAsync(id);
        if (producto != null)
        {
            _contexto.Productos.Remove(producto);
            await _contexto.SaveChangesAsync();
        }
    }
}
