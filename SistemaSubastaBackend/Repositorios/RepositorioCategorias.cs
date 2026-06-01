using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioCategorias : IRepositorioCategorias
{
    private readonly ContextoSubastas _contexto;

    public RepositorioCategorias(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Categoria>> ObtenerTodasAsync()
    {
        return await _contexto.Categorias.ToListAsync();
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Categorias.FindAsync(id);
    }

    public async Task<Categoria> CrearAsync(Categoria categoria)
    {
        _contexto.Categorias.Add(categoria);
        await _contexto.SaveChangesAsync();
        return categoria;
    }

    public async Task ActualizarAsync(Categoria categoria)
    {
        _contexto.Categorias.Update(categoria);
        await _contexto.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var categoria = await _contexto.Categorias.FindAsync(id);
        if (categoria != null)
        {
            _contexto.Categorias.Remove(categoria);
            await _contexto.SaveChangesAsync();
        }
    }
}
