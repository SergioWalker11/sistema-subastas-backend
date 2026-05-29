using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioImagenes
{
    Task<ImagenProducto> SubirImagenAsync(int productoId, IFormFile archivo, bool esPrincipal);
    Task<List<ImagenProducto>> ObtenerPorProductoAsync(int productoId);
    Task EliminarAsync(int id);
}
