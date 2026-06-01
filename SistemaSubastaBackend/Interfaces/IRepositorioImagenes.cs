using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IRepositorioImagenes
{
    Task<ImagenProducto> CrearAsync(ImagenProducto imagen);
    Task<List<ImagenProducto>> ObtenerPorProductoAsync(int productoId);
    Task EliminarAsync(int id);
}
