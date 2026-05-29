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
                    subasta.Estado = "finalizada";
                    await repositorio.ActualizarAsync(subasta);

                    var ganadora = await repoPujas.ObtenerUltimaPujaAsync(subasta.Id);
                    if (ganadora != null)
                    {
                        await notificaciones.NotificarSubastaGanadaAsync(ganadora.UsuarioId, subasta.Id);
                        await notificaciones.NotificarSubastaGanadaAsync(subasta.VendedorId, subasta.Id);
                    }
                }
            }
            catch { }

            await Task.Delay(Intervalo, token);
        }
    }
}
