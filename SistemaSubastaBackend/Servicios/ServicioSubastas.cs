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
        ValidarCreacion(dto);

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

    private static void ValidarCreacion(SubastaCrearDTO dto)
    {
        var reglas = new (bool falla, string mensaje)[]
        {
            (dto.PrecioInicial <= 0, "El precio inicial debe ser mayor a cero"),
            (dto.FechaInicio == default, "La fecha de inicio es obligatoria"),
            (dto.FechaFin <= dto.FechaInicio, "La fecha de fin debe ser posterior a la fecha de inicio"),
        };

        var errores = reglas.Where(r => r.falla).Select(r => r.mensaje).ToList();
        if (errores.Count > 0) throw new ArgumentException(string.Join(", ", errores));
    }

    public async Task<Subasta> ActualizarEstadoAsync(int id, string estado)
    {
        var subasta = await _repositorioSubastas.ObtenerPorIdAsync(id);
        if (subasta == null)
            throw new KeyNotFoundException($"No se encontro la subasta con ID {id}");

        subasta.Estado = estado;
        return await _repositorioSubastas.ActualizarAsync(subasta);
    }

    public async Task<Subasta> EditarSubastaAsync(int id, SubastaEditarDTO dto)
    {
        var subasta = await _repositorioSubastas.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontro la subasta con ID {id}");

        if (subasta.FechaInicio <= DateTime.UtcNow)
            throw new InvalidOperationException("No se puede editar una subasta que ya inicio");

        if (dto.PrecioInicial <= 0)
            throw new ArgumentException("El precio inicial debe ser mayor a cero");

        if (dto.FechaFin <= dto.FechaInicio)
            throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio");

        subasta.PrecioInicial = dto.PrecioInicial;
        subasta.PrecioActual = dto.PrecioInicial;
        subasta.FechaInicio = dto.FechaInicio;
        subasta.FechaFin = dto.FechaFin;

        return await _repositorioSubastas.ActualizarAsync(subasta);
    }

    public async Task<Subasta> CancelarSubastaAsync(int id, int vendedorId)
    {
        var subasta = await _repositorioSubastas.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontro la subasta con ID {id}");

        if (subasta.VendedorId != vendedorId)
            throw new UnauthorizedAccessException("Solo el vendedor puede cancelar su propia subasta");

        subasta.Estado = "cancelada";
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

        var estadosValidos = new[] { "pendiente_pago", "vendida", "incumplida" };

        return subastas
            .Where(s => estadosValidos.Contains(s.Estado) && s.Pujas.Any())
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
                FechaLimitePago = x.Subasta.FechaLimitePago,
                Estado = x.Subasta.Estado,
                NombreVendedor = x.Subasta.Vendedor?.NombreCompleto ?? string.Empty,
                CorreoVendedor = x.Subasta.Vendedor?.Correo ?? string.Empty,
                Pagado = pagosSubastaIds.Contains(x.Subasta.Id)
            })
            .ToList();
    }

    public async Task<List<SubastaGanadaDTO>> ListarPendientesPagoAsync(int usuarioId)
    {
        var ganadas = await ListarGanadasPorUsuarioAsync(usuarioId);
        return ganadas.Where(g => !g.Pagado && g.Estado == "pendiente_pago").ToList();
    }

    public async Task<List<VentaDTO>> ListarVentasAsync(int vendedorId)
    {
        var subastas = await _repositorioSubastas.ObtenerTodasConPujasAsync();
        var todasLasVentas = subastas.Where(s =>
            s.VendedorId == vendedorId &&
            new[] { "pendiente_pago", "vendida", "incumplida" }.Contains(s.Estado));

        var ventas = new List<VentaDTO>();

        foreach (var subasta in todasLasVentas)
        {
            var ultimaPuja = subasta.Pujas.OrderByDescending(p => p.FechaCreacion).FirstOrDefault();
            var pago = subasta.Pagos.OrderByDescending(p => p.FechaPago).FirstOrDefault();

            ventas.Add(new VentaDTO
            {
                SubastaId = subasta.Id,
                NombreProducto = subasta.Producto?.Nombre ?? string.Empty,
                NombreGanador = ultimaPuja?.Usuario?.NombreCompleto,
                CorreoGanador = ultimaPuja?.Usuario?.Correo,
                PrecioFinal = ultimaPuja?.Monto ?? subasta.PrecioActual,
                Estado = subasta.Estado,
                FechaFin = subasta.FechaFin,
                FechaLimitePago = subasta.FechaLimitePago,
                FechaPago = pago?.FechaPago
            });
        }

        return ventas;
    }

    private SubastaDetalleDTO MapearADetalleDTO(Subasta subasta, int cantidadPujas)
    {
        var imagenPrincipal = subasta.Producto?.Imagenes?.FirstOrDefault(i => i.EsPrincipal)?.RutaArchivo
                           ?? subasta.Producto?.Imagenes?.FirstOrDefault()?.RutaArchivo;

        return new SubastaDetalleDTO
        {
            Id = subasta.Id,
            ProductoId = subasta.ProductoId,
            NombreProducto = subasta.Producto.Nombre,
            DescripcionProducto = subasta.Producto.Descripcion,
            VendedorId = subasta.VendedorId,
            NombreVendedor = subasta.Vendedor.NombreCompleto,
            GanadorId = subasta.GanadorId,
            NombreGanador = subasta.Ganador?.NombreCompleto,
            PrecioInicial = subasta.PrecioInicial,
            PrecioActual = subasta.PrecioActual,
            FechaInicio = subasta.FechaInicio,
            FechaFin = subasta.FechaFin,
            FechaLimitePago = subasta.FechaLimitePago,
            Estado = subasta.Estado,
            CantidadPujas = cantidadPujas,
            CategoriaId = subasta.Producto.CategoriaId ?? 0,
            CategoriaNombre = subasta.Producto.Categoria?.Nombre ?? string.Empty,
            ImagenPrincipal = imagenPrincipal
        };
    }
}
