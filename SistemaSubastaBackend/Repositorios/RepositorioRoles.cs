using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioRoles : IRepositorioRoles
{
    private readonly ContextoSubastas _contexto;

    public RepositorioRoles(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Rol>> ObtenerTodosAsync()
    {
        return await _contexto.Roles.ToListAsync();
    }

    public async Task<Rol?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Roles.FindAsync(id);
    }
}
