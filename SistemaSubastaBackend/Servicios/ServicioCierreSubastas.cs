using SistemaSubastaBackend.Interfaces;

namespace SistemaSubastaBackend.Servicios;

public class ServicioCierreSubastas : BackgroundService
{
    private readonly IServiceScopeFactory _fabricaAlcance;
    private readonly ILogger<ServicioCierreSubastas> _logger;
    private static readonly TimeSpan Intervalo = TimeSpan.FromMinutes(1);

    public ServicioCierreSubastas(IServiceScopeFactory fabricaAlcance, ILogger<ServicioCierreSubastas> logger)
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
                var repoPujas = alcance.ServiceProvider.GetRequiredService<IRepositorioPujas>();
                var notificaciones = alcance.ServiceProvider.GetRequiredService<IServicioNotificaciones>();

                var activas = await repositorio.ObtenerPorEstadoAsync("activa");
                var ahora = DateTime.UtcNow;

                foreach (var subasta in activas.Where(s => s.FechaFin <= ahora))
                {
                    var ultimaPuja = await repoPujas.ObtenerUltimaPujaAsync(subasta.Id);

                    if (ultimaPuja != null)
                    {
                        subasta.GanadorId = ultimaPuja.UsuarioId;
                        subasta.Estado = "pendiente_pago";
                        subasta.FechaLimitePago = ahora.AddHours(24);

                        await repositorio.ActualizarAsync(subasta);
                        await notificaciones.NotificarSubastaGanadaAsync(ultimaPuja.UsuarioId, subasta.Id);
                    }
                    else
                    {
                        subasta.Estado = "cancelada";
                        await repositorio.ActualizarAsync(subasta);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ciclo de cierre de subastas");
            }

            await Task.Delay(Intervalo, token);
        }
    }
}
