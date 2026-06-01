using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Repositorios;

public class RepositorioImagenes : IRepositorioImagenes
{
    private readonly ContextoSubastas _contexto;

    public RepositorioImagenes(ContextoSubastas contexto)
    {
        _contexto = contexto;
    }

    public async Task<ImagenProducto> CrearAsync(ImagenProducto imagen)
    {
        _contexto.ImagenesProducto.Add(imagen);
        await _contexto.SaveChangesAsync();
        return imagen;
    }

    public async Task<List<ImagenProducto>> ObtenerPorProductoAsync(int productoId)
    {
        return await _contexto.ImagenesProducto
            .Where(i => i.ProductoId == productoId)
            .OrderByDescending(i => i.EsPrincipal)
            .ThenBy(i => i.Orden)
            .ToListAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var imagen = await _contexto.ImagenesProducto.FindAsync(id);
        if (imagen != null)
        {
            _contexto.ImagenesProducto.Remove(imagen);
            await _contexto.SaveChangesAsync();
        }
    }
}
