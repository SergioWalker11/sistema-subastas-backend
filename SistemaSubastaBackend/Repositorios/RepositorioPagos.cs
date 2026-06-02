using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioPagos : IRepositorioPagos
{
    private readonly ContextoSubastas _contexto;

    public RepositorioPagos(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<Pago> CrearAsync(Pago pago)
    {
        _contexto.Pagos.Add(pago);
        await _contexto.SaveChangesAsync();
        return pago;
    }

    public async Task<Pago?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Pagos
            .Include(p => p.Usuario)
            .Include(p => p.Subasta)
                .ThenInclude(s => s.Vendedor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Pago>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        return await _contexto.Pagos
            .Include(p => p.Usuario)
            .Include(p => p.Subasta)
                .ThenInclude(s => s.Vendedor)
            .Where(p => p.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<List<Pago>> ObtenerPorSubastaAsync(int subastaId)
    {
        return await _contexto.Pagos
            .Where(p => p.SubastaId == subastaId)
            .ToListAsync();
    }

    public async Task<Pago> ActualizarAsync(Pago pago)
    {
        _contexto.Pagos.Update(pago);
        await _contexto.SaveChangesAsync();
        return pago;
    }
}
