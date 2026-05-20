using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioUsuarios : IRepositorioUsuarios
{
    private readonly ContextoSubastas _contexto;

    public RepositorioUsuarios(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Usuarios.FindAsync(id);
    }

    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
    {
        return await _contexto.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == correo);
    }

    public async Task<Usuario> CrearAsync(Usuario usuario)
    {
        _contexto.Usuarios.Add(usuario);
        await _contexto.SaveChangesAsync();
        return usuario;
    }
}
