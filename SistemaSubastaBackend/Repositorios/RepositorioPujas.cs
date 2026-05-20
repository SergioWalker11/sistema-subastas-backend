using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioPujas : IRepositorioPujas
{
    private readonly ContextoSubastas _contexto;

    public RepositorioPujas(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Puja>> ObtenerPorSubastaAsync(int subastaId)
    {
        return await _contexto.Pujas
            .Include(p => p.Usuario)
            .Where(p => p.SubastaId == subastaId)
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Puja> CrearAsync(Puja puja)
    {
        _contexto.Pujas.Add(puja);
        await _contexto.SaveChangesAsync();
        return puja;
    }

    public async Task<Puja?> ObtenerUltimaPujaAsync(int subastaId)
    {
        return await _contexto.Pujas
            .Where(p => p.SubastaId == subastaId)
            .OrderByDescending(p => p.FechaCreacion)
            .FirstOrDefaultAsync();
    }

    public async Task<int> ContarPujasAsync(int subastaId)
    {
        return await _contexto.Pujas
            .CountAsync(p => p.SubastaId == subastaId);
    }
}
