using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Servicios;

public class ServicioSubastas : IServicioSubastas
{
    private readonly IRepositorioSubastas _repositorioSubastas;
    private readonly IRepositorioPujas _repositorioPujas;

    public ServicioSubastas(IRepositorioSubastas repositorioSubastas, IRepositorioPujas repositorioPujas)
    {
        _repositorioSubastas = repositorioSubastas;
        _repositorioPujas = repositorioPujas;
    }

    public async Task<List<SubastaDetalleDTO>> ListarSubastasAsync()
    {
        var subastas = await _repositorioSubastas.ObtenerTodasAsync();
        var resultado = new List<SubastaDetalleDTO>();

        foreach (var subasta in subastas)
        {
            var cantidadPujas = await _repositorioPujas.ContarPujasAsync(subasta.Id);
            resultado.Add(MapearADetalleDTO(subasta, cantidadPujas));
        }

        return resultado;
    }

    public async Task<SubastaDetalleDTO?> ObtenerDetalleAsync(int id)
    {
        var subasta = await _repositorioSubastas.ObtenerPorIdAsync(id);
        if (subasta == null) return null;

        var cantidadPujas = await _repositorioPujas.ContarPujasAsync(subasta.Id);
        return MapearADetalleDTO(subasta, cantidadPujas);
    }

    public async Task<Subasta> CrearSubastaAsync(SubastaCrearDTO dto)
    {
        var subasta = new Subasta
        {
            ProductoId = dto.ProductoId,
            PrecioInicial = dto.PrecioInicial,
            PrecioActual = dto.PrecioInicial,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            Estado = "activa"
        };

        return await _repositorioSubastas.CrearAsync(subasta);
    }

    public async Task<Subasta> ActualizarEstadoAsync(int id, string estado)
    {
        var subasta = await _repositorioSubastas.ObtenerPorIdAsync(id);
        if (subasta == null)
        {
            throw new KeyNotFoundException($"No se encontro la subasta con ID {id}");
        }

        subasta.Estado = estado;
        return await _repositorioSubastas.ActualizarAsync(subasta);
    }

    private SubastaDetalleDTO MapearADetalleDTO(Subasta subasta, int cantidadPujas)
    {
        return new SubastaDetalleDTO
        {
            Id = subasta.Id,
            NombreProducto = subasta.Producto.Nombre,
            DescripcionProducto = subasta.Producto.Descripcion,
            PrecioInicial = subasta.PrecioInicial,
            PrecioActual = subasta.PrecioActual,
            FechaInicio = subasta.FechaInicio,
            FechaFin = subasta.FechaFin,
            Estado = subasta.Estado,
            CantidadPujas = cantidadPujas
        };
    }
}
