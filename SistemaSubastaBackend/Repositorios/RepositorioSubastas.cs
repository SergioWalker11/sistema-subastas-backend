using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioSubastas : IRepositorioSubastas
{
    private readonly ContextoSubastas _contexto;

    public RepositorioSubastas(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Subasta>> ObtenerTodasAsync()
    {
        return await _contexto.Subastas
            .Include(s => s.Producto)
            .Include(s => s.Vendedor)
            .ToListAsync();
    }

    public async Task<Subasta?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Subastas
            .Include(s => s.Producto)
            .Include(s => s.Vendedor)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Subasta> CrearAsync(Subasta subasta)
    {
        _contexto.Subastas.Add(subasta);
        await _contexto.SaveChangesAsync();
        return subasta;
    }

    public async Task<Subasta> ActualizarAsync(Subasta subasta)
    {
        _contexto.Subastas.Update(subasta);
        await _contexto.SaveChangesAsync();
        return subasta;
    }

    public async Task<List<Subasta>> ObtenerPorEstadoAsync(string estado)
    {
        return await _contexto.Subastas
            .Include(s => s.Producto)
            .Include(s => s.Vendedor)
            .Where(s => s.Estado == estado)
            .ToListAsync();
    }

    public async Task<List<Subasta>> ObtenerPorVendedorAsync(int vendedorId)
    {
        return await _contexto.Subastas
            .Include(s => s.Producto)
            .Include(s => s.Vendedor)
            .Include(s => s.Pujas)
            .Where(s => s.VendedorId == vendedorId)
            .ToListAsync();
    }

    public async Task<List<Subasta>> ObtenerTodasConPujasAsync()
    {
        return await _contexto.Subastas
            .Include(s => s.Producto)
            .Include(s => s.Vendedor)
            .Include(s => s.Pujas)
                .ThenInclude(p => p.Usuario)
            .ToListAsync();
    }
}
