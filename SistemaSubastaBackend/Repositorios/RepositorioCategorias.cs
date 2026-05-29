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
}
