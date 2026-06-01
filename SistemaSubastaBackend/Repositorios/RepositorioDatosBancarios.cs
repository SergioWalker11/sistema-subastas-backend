using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioDatosBancarios : IRepositorioDatosBancarios
{
    private readonly ContextoSubastas _contexto;

    public RepositorioDatosBancarios(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<DatosBancarios?> ObtenerPorUsuarioAsync(int usuarioId)
    {
        return await _contexto.DatosBancarios.FirstOrDefaultAsync(d => d.UsuarioId == usuarioId);
    }

    public async Task<DatosBancarios> GuardarAsync(DatosBancarios datos)
    {
        var existente = await _contexto.DatosBancarios.FirstOrDefaultAsync(d => d.UsuarioId == datos.UsuarioId);
        if (existente != null)
        {
            existente.Banco = datos.Banco;
            existente.TipoCuenta = datos.TipoCuenta;
            existente.NumeroCuenta = datos.NumeroCuenta;
            existente.Titular = datos.Titular;
            _contexto.DatosBancarios.Update(existente);
        }
        else
        {
            _contexto.DatosBancarios.Add(datos);
        }
        await _contexto.SaveChangesAsync();
        return existente ?? datos;
    }
}
