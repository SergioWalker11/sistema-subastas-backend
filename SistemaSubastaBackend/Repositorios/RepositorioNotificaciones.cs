using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioNotificaciones : IRepositorioNotificaciones
{
    private readonly ContextoSubastas _contexto;

    public RepositorioNotificaciones(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Notificacion>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        return await _contexto.Notificaciones
            .Where(n => n.UsuarioId == usuarioId)
            .OrderByDescending(n => n.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Notificacion> CrearAsync(Notificacion notificacion)
    {
        _contexto.Notificaciones.Add(notificacion);
        await _contexto.SaveChangesAsync();
        return notificacion;
    }

    public async Task<Notificacion> MarcarComoLeidaAsync(int id)
    {
        var notificacion = await _contexto.Notificaciones.FindAsync(id);
        if (notificacion != null)
        {
            notificacion.Leida = true;
            await _contexto.SaveChangesAsync();
        }
        return notificacion!;
    }

    public async Task<int> ContarNoLeidasAsync(int usuarioId)
    {
        return await _contexto.Notificaciones
            .CountAsync(n => n.UsuarioId == usuarioId && !n.Leida);
    }
}
