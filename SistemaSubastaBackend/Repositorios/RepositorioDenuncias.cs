using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioDenuncias : IRepositorioDenuncias
{
    private readonly ContextoSubastas _contexto;

    public RepositorioDenuncias(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<Denuncia> CrearAsync(Denuncia denuncia)
    {
        _contexto.Denuncias.Add(denuncia);
        await _contexto.SaveChangesAsync();
        return denuncia;
    }

    public async Task<List<Denuncia>> ObtenerTodasAsync()
    {
        return await _contexto.Denuncias
            .Include(d => d.Denunciante)
            .Include(d => d.Denunciado)
            .OrderByDescending(d => d.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Denuncia?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Denuncias.FindAsync(id);
    }

    public async Task<Denuncia> ActualizarAsync(Denuncia denuncia)
    {
        _contexto.Denuncias.Update(denuncia);
        await _contexto.SaveChangesAsync();
        return denuncia;
    }
}
