namespace SistemaSubastaBackend.ServiciosExternos;

public class ServicioPasarelaPagos
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

    private string GenerarCodigoTransaccion()
    {
        var prefijo = "SUBASTA";
        var marcaTiempo = DateTime.Now.ToString("yyyyMMddHHmmss");
        var aleatorio = new Random().Next(1000, 9999);
        return $"{prefijo}-{marcaTiempo}-{aleatorio}";
    }
}

public class ResultadoPasarela
{
    public string CodigoTransaccion { get; set; } = string.Empty;
    public bool Aprobado { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}
