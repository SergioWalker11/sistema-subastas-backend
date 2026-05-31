using SistemaSubastaBackend.Interfaces;

namespace SistemaSubastaBackend.Servicios;

public class ServicioPagosVencidos : BackgroundService
{
    private readonly IServiceScopeFactory _fabricaAlcance;
    private readonly ILogger<ServicioPagosVencidos> _logger;
    private static readonly TimeSpan Intervalo = TimeSpan.FromMinutes(1);

    public ServicioPagosVencidos(IServiceScopeFactory fabricaAlcance, ILogger<ServicioPagosVencidos> logger)
    {
        _fabricaAlcance = fabricaAlcance;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                using var alcance = _fabricaAlcance.CreateScope();
                var repositorio = alcance.ServiceProvider.GetRequiredService<IRepositorioSubastas>();
                var notificaciones = alcance.ServiceProvider.GetRequiredService<IServicioNotificaciones>();

                var pendientes = await repositorio.ObtenerPorEstadoAsync("pendiente_pago");
                var ahora = DateTime.UtcNow;

                foreach (var subasta in pendientes.Where(s => s.FechaLimitePago.HasValue && s.FechaLimitePago.Value <= ahora))
                {
                    subasta.Estado = "incumplida";
                    await repositorio.ActualizarAsync(subasta);

                    var nombreProducto = subasta.Producto?.Nombre ?? "Desconocido";

                    if (subasta.GanadorId.HasValue)
                    {
                        await notificaciones.NotificarIncumplimientoPagoAsync(
                            subasta.GanadorId.Value, subasta.VendedorId, nombreProducto);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ciclo de verificacion de pagos vencidos");
            }

            await Task.Delay(Intervalo, token);
        }
    }
}
