using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Servicios;

public class ServicioPagos : IServicioPagos
{
    private readonly IRepositorioPagos _repositorioPagos;
    private readonly IRepositorioSubastas _repositorioSubastas;
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly IServicioPasarelaPagos _pasarelaPagos;
    private readonly IServicioNotificaciones _servicioNotificaciones;

    public ServicioPagos(
        IRepositorioPagos repositorioPagos,
        IRepositorioSubastas repositorioSubastas,
        IRepositorioUsuarios repositorioUsuarios,
        IServicioPasarelaPagos pasarelaPagos,
        IServicioNotificaciones servicioNotificaciones)
    {
        _repositorioPagos = repositorioPagos;
        _repositorioSubastas = repositorioSubastas;
        _repositorioUsuarios = repositorioUsuarios;
        _pasarelaPagos = pasarelaPagos;
        _servicioNotificaciones = servicioNotificaciones;
    }

    public async Task<PagoRespuestaDTO> ProcesarPagoConTarjetaAsync(PagoTarjetaDTO dto)
    {
        var subasta = await _repositorioSubastas.ObtenerPorIdAsync(dto.SubastaId)
            ?? throw new KeyNotFoundException($"No se encontro la subasta con ID {dto.SubastaId}");
        var usuario = await _repositorioUsuarios.ObtenerPorIdAsync(dto.UsuarioId)
            ?? throw new KeyNotFoundException($"No se encontro el usuario con ID {dto.UsuarioId}");

        if (usuario.EstaSuspendido)
            throw new InvalidOperationException("Tu cuenta esta suspendida. No puedes realizar pagos.");

        ValidarPago(subasta, usuario, dto.UsuarioId);
        ValidarMonto(dto.Monto);

        var resultado = await _pasarelaPagos.ProcesarPagoConTarjetaAsync(dto);
        if (!resultado.Aprobado)
            throw new InvalidOperationException(resultado.Mensaje);

        var pago = new Pago
        {
            SubastaId = dto.SubastaId,
            UsuarioId = dto.UsuarioId,
            Monto = dto.Monto,
            CodigoTransaccion = resultado.CodigoTransaccion,
            EstadoPago = "custodia",
            FechaPago = DateTime.UtcNow
        };

        pago = await _repositorioPagos.CrearAsync(pago);

        subasta.Estado = "pagada";
        await _repositorioSubastas.ActualizarAsync(subasta);

        await _servicioNotificaciones.NotificarPagoRecibidoAsync(
            usuario.Id,
            subasta.VendedorId,
            subasta.Producto?.Nombre ?? "Desconocido");

        ProgramarLiberacionFondos(dto.SubastaId, subasta.VendedorId, subasta.Vendedor?.Correo ?? "");

        return new PagoRespuestaDTO
        {
            Id = pago.Id,
            SubastaId = pago.SubastaId,
            NombreUsuario = usuario.NombreCompleto,
            Monto = pago.Monto,
            CodigoTransaccion = pago.CodigoTransaccion,
            EstadoPago = pago.EstadoPago,
            FechaPago = pago.FechaPago,
            Franquicia = resultado.Franquicia,
            UltimosDigitos = resultado.UltimosDigitos
        };
    }

    public async Task<PagoRespuestaDTO> ProcesarPagoAsync(PagoCrearDTO dto)
    {
        var subasta = await _repositorioSubastas.ObtenerPorIdAsync(dto.SubastaId)
            ?? throw new KeyNotFoundException($"No se encontro la subasta con ID {dto.SubastaId}");
        var usuario = await _repositorioUsuarios.ObtenerPorIdAsync(dto.UsuarioId)
            ?? throw new KeyNotFoundException($"No se encontro el usuario con ID {dto.UsuarioId}");

        ValidarPago(subasta, usuario, dto.UsuarioId);
        ValidarMonto(dto.Monto);

        var resultado = await _pasarelaPagos.ProcesarPagoAsync(dto.Monto, usuario.NombreCompleto, usuario.Correo);
        if (!resultado.Aprobado)
            throw new InvalidOperationException($"Pago rechazado: {resultado.Mensaje}");

        var pago = new Pago
        {
            SubastaId = dto.SubastaId,
            UsuarioId = dto.UsuarioId,
            Monto = dto.Monto,
            CodigoTransaccion = resultado.CodigoTransaccion,
            EstadoPago = "aprobado",
            FechaPago = DateTime.UtcNow
        };

        pago = await _repositorioPagos.CrearAsync(pago);

        subasta.Estado = "vendida";
        await _repositorioSubastas.ActualizarAsync(subasta);

        await _servicioNotificaciones.NotificarPagoRecibidoAsync(
            usuario.Id,
            subasta.VendedorId,
            subasta.Producto?.Nombre ?? "Desconocido");

        return new PagoRespuestaDTO
        {
            Id = pago.Id,
            SubastaId = pago.SubastaId,
            NombreUsuario = usuario.NombreCompleto,
            Monto = pago.Monto,
            CodigoTransaccion = pago.CodigoTransaccion,
            EstadoPago = pago.EstadoPago,
            FechaPago = pago.FechaPago
        };
    }

    public async Task<PagoRespuestaDTO?> ObtenerPagoAsync(int id)
    {
        var pago = await _repositorioPagos.ObtenerPorIdAsync(id);
        if (pago == null) return null;

        return MapearARespuestaDTO(pago);
    }

    public async Task<List<PagoRespuestaDTO>> ObtenerPagosUsuarioAsync(int usuarioId)
    {
        var pagos = await _repositorioPagos.ObtenerPorUsuarioAsync(usuarioId);
        return pagos.Select(MapearARespuestaDTO).ToList();
    }

    private static void ValidarPago(Subasta subasta, Usuario usuario, int usuarioId)
    {
        if (usuario.Rol?.Nombre == "administrador")
            throw new InvalidOperationException("El administrador no puede manipular pagos");
        if (subasta.Estado != "pendiente_pago")
            throw new InvalidOperationException($"No se puede pagar una subasta en estado '{subasta.Estado}'");
        if (subasta.GanadorId != usuarioId)
            throw new InvalidOperationException("Solo el ganador de la subasta puede realizar el pago.");
        if (subasta.FechaLimitePago.HasValue && DateTime.UtcNow > subasta.FechaLimitePago.Value)
            throw new InvalidOperationException("El plazo de pago ha vencido.");
    }

    private static void ValidarMonto(decimal monto)
    {
        var errores = ValidadorEntrada.ValidarMonto(monto);
        if (errores.Count > 0) throw new ArgumentException(string.Join(", ", errores));
    }

    private static void ProgramarLiberacionFondos(int subastaId, int vendedorId, string correoVendedor)
    {
        _ = Task.Run(async () =>
        {
            await Task.Delay(30_000);
            var alcance = AppServiciosProveedor.Instancia?.CreateScope();
            if (alcance == null) return;

            var repoSubastas = alcance.ServiceProvider.GetRequiredService<IRepositorioSubastas>();
            var repoPagos = alcance.ServiceProvider.GetRequiredService<IRepositorioPagos>();
            var repoBanco = alcance.ServiceProvider.GetRequiredService<IRepositorioDatosBancarios>();
            var pasarela = alcance.ServiceProvider.GetRequiredService<IServicioPasarelaPagos>();
            var notif = alcance.ServiceProvider.GetRequiredService<IServicioNotificaciones>();

            var pago = (await repoPagos.ObtenerPorSubastaAsync(subastaId)).FirstOrDefault(p => p.EstadoPago == "custodia");
            if (pago == null) return;

            var datosBancarios = await repoBanco.ObtenerPorUsuarioAsync(vendedorId);

            if (datosBancarios != null)
            {
                await pasarela.ProcesarDepositoAsync(pago.Monto, datosBancarios.Banco, datosBancarios.NumeroCuenta, datosBancarios.Titular);
            }

            pago.EstadoPago = "depositado";
            await repoPagos.ActualizarAsync(pago);

            var subasta = await repoSubastas.ObtenerPorIdAsync(subastaId);
            if (subasta != null && subasta.Estado == "pagada")
            {
                subasta.Estado = "vendida";
                await repoSubastas.ActualizarAsync(subasta);
            }

            await notif.NotificarDepositoRealizadoAsync(vendedorId, datosBancarios?.Banco ?? "cuenta registrada");
        });
    }

    private PagoRespuestaDTO MapearARespuestaDTO(Pago pago)
    {
        return new PagoRespuestaDTO
        {
            Id = pago.Id,
            SubastaId = pago.SubastaId,
            NombreUsuario = pago.Usuario.NombreCompleto,
            Monto = pago.Monto,
            CodigoTransaccion = pago.CodigoTransaccion,
            EstadoPago = pago.EstadoPago,
            FechaPago = pago.FechaPago
        };
    }
}
