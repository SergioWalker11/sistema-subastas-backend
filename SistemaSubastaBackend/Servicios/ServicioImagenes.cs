using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Servicios;

public class ServicioImagenes : IServicioImagenes
{
    private readonly string _rutaBase;

    public ServicioImagenes(IConfiguration configuracion)
    {
        _rutaBase = Path.Combine(Directory.GetCurrentDirectory(), "ImagenesProductos");
    }

    public async Task<ImagenProducto> SubirImagenAsync(int productoId, IFormFile archivo, bool esPrincipal)
    {
        if (archivo == null || archivo.Length == 0)
            throw new ArgumentException("Archivo no proporcionado");

        var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(archivo.FileName).ToLower();
        if (!extensionesPermitidas.Contains(extension))
            throw new ArgumentException("Solo se permiten archivos JPG, PNG y WebP");

        if (!Directory.Exists(_rutaBase))
            Directory.CreateDirectory(_rutaBase);

        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaCompleta = Path.Combine(_rutaBase, nombreArchivo);

        using var stream = new FileStream(rutaCompleta, FileMode.Create);
        await archivo.CopyToAsync(stream);

        return new ImagenProducto
        {
            ProductoId = productoId,
            RutaArchivo = nombreArchivo,
            EsPrincipal = esPrincipal,
            Orden = 0
        };
    }

    public Task<List<ImagenProducto>> ObtenerPorProductoAsync(int productoId)
    {
        return Task.FromResult(new List<ImagenProducto>());
    }

    public Task EliminarAsync(int id)
    {
        return Task.CompletedTask;
    }
}
