using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Servicios;

public class ServicioPujas : IServicioPujas
{
    private readonly IRepositorioPujas _repositorioPujas;
    private readonly IRepositorioSubastas _repositorioSubastas;
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly ValidadorPujas _validadorPujas;
    private readonly IServicioNotificaciones _servicioNotificaciones;

    public ServicioPujas(
        IRepositorioPujas repositorioPujas,
        IRepositorioSubastas repositorioSubastas,
        IRepositorioUsuarios repositorioUsuarios,
        IServicioNotificaciones servicioNotificaciones)
    {
        _repositorioPujas = repositorioPujas;
        _repositorioSubastas = repositorioSubastas;
        _repositorioUsuarios = repositorioUsuarios;
        _servicioNotificaciones = servicioNotificaciones;
        _validadorPujas = new ValidadorPujas();
    }

    public async Task<PujaRespuestaDTO> RegistrarPujaAsync(PujaCrearDTO dto)
    {
        var subasta = await _repositorioSubastas.ObtenerPorIdAsync(dto.SubastaId);
        if (subasta == null)
            throw new KeyNotFoundException($"No se encontro la subasta con ID {dto.SubastaId}");

        var usuario = await _repositorioUsuarios.ObtenerPorIdAsync(dto.UsuarioId);
        if (usuario == null)
            throw new KeyNotFoundException($"No se encontro el usuario con ID {dto.UsuarioId}");

        if (usuario.Rol?.Nombre == "administrador")
            throw new InvalidOperationException("El administrador no puede participar en subastas");

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

    public async Task<List<PujaRespuestaDTO>> ObtenerHistorialAsync(int subastaId)
    {
        var pujas = await _repositorioPujas.ObtenerPorSubastaAsync(subastaId);

        return pujas.Select(p => new PujaRespuestaDTO
        {
            Id = p.Id,
            SubastaId = p.SubastaId,
            NombreUsuario = p.Usuario.NombreCompleto,
            Monto = p.Monto,
            FechaCreacion = p.FechaCreacion
        }).ToList();
    }
}
