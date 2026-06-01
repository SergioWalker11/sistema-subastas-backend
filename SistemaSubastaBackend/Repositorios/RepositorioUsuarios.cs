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

    public async Task<List<Usuario>> ObtenerTodosAsync()
    {
        return await _contexto.Usuarios
            .Include(u => u.Rol)
            .ToListAsync();
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
    {
        return await _contexto.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Correo == correo);
    }

    public async Task<Usuario> CrearAsync(Usuario usuario)
    {
        _contexto.Usuarios.Add(usuario);
        await _contexto.SaveChangesAsync();

        return await _contexto.Usuarios
            .Include(u => u.Rol)
            .FirstAsync(u => u.Id == usuario.Id);
    }

    public async Task<Usuario> ActualizarAsync(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
        await _contexto.SaveChangesAsync();
        return usuario;
    }
}
