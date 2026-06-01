using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Servicios;

public class ServicioPujas : IServicioPujas
{
    private readonly IRepositorioPujas _repositorioPujas;
    private readonly IRepositorioSubastas _repositorioSubastas;
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly IValidadorPujas _validadorPujas;
    private readonly IServicioNotificaciones _servicioNotificaciones;

    public ServicioPujas(
        IRepositorioPujas repositorioPujas,
        IRepositorioSubastas repositorioSubastas,
        IRepositorioUsuarios repositorioUsuarios,
        IValidadorPujas validadorPujas,
        IServicioNotificaciones servicioNotificaciones)
    {
        _repositorioPujas = repositorioPujas;
        _repositorioSubastas = repositorioSubastas;
        _repositorioUsuarios = repositorioUsuarios;
        _validadorPujas = validadorPujas;
        _servicioNotificaciones = servicioNotificaciones;
    }

    public async Task<PujaRespuestaDTO> RegistrarPujaAsync(PujaCrearDTO dto)
    {
        var subasta = await _repositorioSubastas.ObtenerPorIdAsync(dto.SubastaId)
            ?? throw new KeyNotFoundException($"No se encontro la subasta con ID {dto.SubastaId}");
        var usuario = await _repositorioUsuarios.ObtenerPorIdAsync(dto.UsuarioId)
            ?? throw new KeyNotFoundException($"No se encontro el usuario con ID {dto.UsuarioId}");

        if (usuario.Rol?.Nombre == "administrador")
            throw new InvalidOperationException("El administrador no puede participar en subastas");

        if (usuario.Rol?.Nombre == "vendedor")
            throw new InvalidOperationException("Los vendedores no pueden realizar pujas. Use su cuenta de comprador.");

        var ultimaPuja = await _repositorioPujas.ObtenerUltimaPujaAsync(dto.SubastaId);
        var errores = _validadorPujas.ValidarPuja(dto.Monto, subasta, ultimaPuja, dto.UsuarioId);
        if (errores.Count > 0)
            throw new ArgumentException(string.Join(", ", errores));

        var puja = new Puja
        {
            SubastaId = dto.SubastaId,
            UsuarioId = dto.UsuarioId,
            Monto = dto.Monto,
            FechaCreacion = DateTime.UtcNow
        };

        puja = await _repositorioPujas.CrearAsync(puja);

        subasta.PrecioActual = _validadorPujas.CalcularNuevoPrecio(dto.Monto, subasta.PrecioActual);
        await _repositorioSubastas.ActualizarAsync(subasta);

        if (ultimaPuja != null && ultimaPuja.UsuarioId != dto.UsuarioId)
        {
            await _servicioNotificaciones.NotificarNuevaPujaAsync(
                ultimaPuja.UsuarioId,
                "Puja superada",
                $"Alguien ha superado tu puja en '{subasta.Producto.Nombre}'");
        }

        await _servicioNotificaciones.NotificarNuevaPujaAsync(
            subasta.VendedorId,
            "Puja recibida en tu subasta",
            $"{usuario.NombreCompleto} ha pujado {dto.Monto:C} en '{subasta.Producto.Nombre}'");

        return new PujaRespuestaDTO
        {
            Id = puja.Id,
            SubastaId = puja.SubastaId,
            NombreUsuario = usuario.NombreCompleto,
            Monto = puja.Monto,
            FechaCreacion = puja.FechaCreacion
        };
    }

    public async Task<List<PujaRespuestaDTO>> ObtenerHistorialAsync(int subastaId, int? usuarioId = null)
    {
        var pujas = await _repositorioPujas.ObtenerPorSubastaAsync(subastaId);

        return pujas.Select(p => new PujaRespuestaDTO
        {
            Id = p.Id,
            SubastaId = p.SubastaId,
            NombreUsuario = p.UsuarioId == usuarioId ? p.Usuario.NombreCompleto : "Anonimo",
            Monto = p.Monto,
            FechaCreacion = p.FechaCreacion,
            EsPropia = p.UsuarioId == usuarioId
        }).ToList();
    }
}
