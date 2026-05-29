using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Servicios;

public class ServicioSubastas : IServicioSubastas
{
    private readonly IRepositorioSubastas _repositorioSubastas;
    private readonly IRepositorioPujas _repositorioPujas;
    private readonly IRepositorioPagos _repositorioPagos;

    public ServicioSubastas(IRepositorioSubastas repositorioSubastas, IRepositorioPujas repositorioPujas, IRepositorioPagos repositorioPagos)
    {
        _repositorioSubastas = repositorioSubastas;
        _repositorioPujas = repositorioPujas;
        _repositorioPagos = repositorioPagos;
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
            VendedorId = dto.VendedorId,
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

    public async Task<List<SubastaDetalleDTO>> ListarPorVendedorAsync(int vendedorId)
    {
        var subastas = await _repositorioSubastas.ObtenerPorVendedorAsync(vendedorId);
        var resultado = new List<SubastaDetalleDTO>();

        foreach (var subasta in subastas)
        {
            var cantidadPujas = subasta.Pujas?.Count ?? 0;
            resultado.Add(MapearADetalleDTO(subasta, cantidadPujas));
        }

        return resultado;
    }

    public async Task<List<SubastaGanadaDTO>> ListarGanadasPorUsuarioAsync(int usuarioId)
    {
        var subastas = await _repositorioSubastas.ObtenerTodasConPujasAsync();
        var pagos = await _repositorioPagos.ObtenerPorUsuarioAsync(usuarioId);
        var pagosSubastaIds = new HashSet<int>(pagos.Select(p => p.SubastaId));

        return subastas
            .Where(s => s.Estado == "finalizada" && s.Pujas.Any())
            .Select(s =>
            {
                var ultimaPuja = s.Pujas.OrderByDescending(p => p.FechaCreacion).First();
                return new { Subasta = s, GanadorId = ultimaPuja.UsuarioId, Monto = ultimaPuja.Monto };
            })
            .Where(x => x.GanadorId == usuarioId)
            .Select(x => new SubastaGanadaDTO
            {
                Id = x.Subasta.Id,
                NombreProducto = x.Subasta.Producto?.Nombre ?? string.Empty,
                MontoGanado = x.Monto,
                FechaFin = x.Subasta.FechaFin,
                NombreVendedor = x.Subasta.Vendedor?.NombreCompleto ?? string.Empty,
                Pagado = pagosSubastaIds.Contains(x.Subasta.Id)
            })
            .ToList();
    }

    private SubastaDetalleDTO MapearADetalleDTO(Subasta subasta, int cantidadPujas)
    {
        return new SubastaDetalleDTO
        {
            Id = subasta.Id,
            NombreProducto = subasta.Producto.Nombre,
            DescripcionProducto = subasta.Producto.Descripcion,
            VendedorId = subasta.VendedorId,
            NombreVendedor = subasta.Vendedor.NombreCompleto,
            PrecioInicial = subasta.PrecioInicial,
            PrecioActual = subasta.PrecioActual,
            FechaInicio = subasta.FechaInicio,
            FechaFin = subasta.FechaFin,
            Estado = subasta.Estado,
            CantidadPujas = cantidadPujas
        };
    }
}
