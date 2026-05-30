using SistemaSubastaBackend.Interfaces;

namespace SistemaSubastaBackend.Servicios;

public class ServicioPagosVencidos : BackgroundService
{
    private readonly IServiceScopeFactory _fabricaAlcance;
    private static readonly TimeSpan Intervalo = TimeSpan.FromMinutes(1);

    public ServicioPagosVencidos(IServiceScopeFactory fabricaAlcance)
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
                        await notificaciones.CrearNotificacionAsync(new DTOs.NotificacionCrearDTO
                        {
                            UsuarioId = subasta.GanadorId.Value,
                            Titulo = "Pago vencido",
                            Mensaje = $"Perdiste la subasta de '{nombreProducto}' por incumplimiento de pago."
                        });
                    }

                    await notificaciones.CrearNotificacionAsync(new DTOs.NotificacionCrearDTO
                    {
                        UsuarioId = subasta.VendedorId,
                        Titulo = "Comprador incumplio",
                        Mensaje = $"El comprador no realizo el pago de '{nombreProducto}' dentro del plazo establecido."
                    });
                }
            }
            catch { }

            await Task.Delay(Intervalo, token);
        }
    }
}
