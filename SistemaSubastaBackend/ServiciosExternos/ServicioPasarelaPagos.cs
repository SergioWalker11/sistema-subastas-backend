using SistemaSubastaBackend.Interfaces;

namespace SistemaSubastaBackend.ServiciosExternos;

public class ServicioPasarelaPagos : IServicioPasarelaPagos
{
    public async Task<ResultadoPasarela> ProcesarPagoAsync(decimal monto, string nombreUsuario, string correo)
    {
        await Task.Delay(500);

        var codigoTransaccion = GenerarCodigoTransaccion();
        var esAprobado = monto > 0 && monto < 1000000;

        return new ResultadoPasarela
        {
            CodigoTransaccion = codigoTransaccion,
            Aprobado = esAprobado,
            Mensaje = esAprobado ? "Pago aprobado exitosamente" : "Pago rechazado",
            Monto = monto
        };
    }

    public async Task<ResultadoPasarela> ConsultarEstadoAsync(string codigoTransaccion)
    {
        await Task.Delay(200);

        return new ResultadoPasarela
        {
            CodigoTransaccion = codigoTransaccion,
            Aprobado = true,
            Mensaje = "Pago confirmado",
            Monto = 0
        };
    }

    private static string GenerarCodigoTransaccion()
    {
        var marcaTiempo = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var aleatorio = Random.Shared.Next(1000, 9999);
        return $"SUBASTA-{marcaTiempo}-{aleatorio}";
    }
}

public class ResultadoPasarela
{
    public string CodigoTransaccion { get; set; } = string.Empty;
    public bool Aprobado { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}
