using SistemaSubastaBackend.Interfaces;

namespace SistemaSubastaBackend.Servicios;

public class ServicioCierreSubastas : BackgroundService
{
    private readonly IServiceScopeFactory _fabricaAlcance;
    private static readonly TimeSpan Intervalo = TimeSpan.FromMinutes(1);

    public ServicioCierreSubastas(IServiceScopeFactory fabricaAlcance)
    {
        _fabricaAlcance = fabricaAlcance;
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

                        await notificaciones.CrearNotificacionAsync(new DTOs.NotificacionCrearDTO
                        {
                            UsuarioId = ultimaPuja.UsuarioId,
                            Titulo = "Subasta ganada",
                            Mensaje = $"Has ganado la subasta. Dispones de 24 horas para realizar el pago."
                        });

                        await notificaciones.CrearNotificacionAsync(new DTOs.NotificacionCrearDTO
                        {
                            UsuarioId = subasta.VendedorId,
                            Titulo = "Subasta finalizada",
                            Mensaje = $"La subasta ha finalizado y existe un comprador pendiente de pago."
                        });
                    }
                    else
                    {
                        subasta.Estado = "cancelada";
                        await repositorio.ActualizarAsync(subasta);
                    }
                }
            }
            catch { }

            await Task.Delay(Intervalo, token);
        }
    }
}
