using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;

namespace SistemaSubastaBackend.ServiciosExternos;

public class ServicioPasarelaPagos : IServicioPasarelaPagos
{
    public async Task<ResultadoPasarela> ProcesarPagoConTarjetaAsync(PagoTarjetaDTO dto)
    {
        await Task.Delay(2000);

        var tarjeta = dto.NumeroTarjeta.Replace("-", "").Replace(" ", "");
        if (tarjeta.Length != 16 || !tarjeta.All(char.IsDigit))
            return new ResultadoPasarela { Aprobado = false, Mensaje = "Numero de tarjeta invalido. Debe tener 16 digitos." };

        var primerDigito = tarjeta[0];
        var franquicia = primerDigito switch
        {
            '4' => "Visa",
            '5' => "Mastercard",
            _ => "Desconocida"
        };

        if (franquicia == "Desconocida")
            return new ResultadoPasarela { Aprobado = false, Mensaje = "Tarjeta no soportada. Use Visa (empieza con 4) o Mastercard (empieza con 5)." };

        if (string.IsNullOrWhiteSpace(dto.Cvv) || dto.Cvv.Length < 3 || !dto.Cvv.All(char.IsDigit))
            return new ResultadoPasarela { Aprobado = false, Mensaje = "CVV invalido. Debe tener 3 digitos." };

        if (string.IsNullOrWhiteSpace(dto.NombreTitular))
            return new ResultadoPasarela { Aprobado = false, Mensaje = "El nombre del titular es obligatorio." };

        var codigo = GenerarCodigoTransaccion();
        var ultimos4 = tarjeta[^4..];

        return new ResultadoPasarela
        {
            CodigoTransaccion = codigo,
            Aprobado = true,
            Mensaje = $"Pago aprobado con {franquicia} terminada en {ultimos4}",
            Monto = dto.Monto,
            Franquicia = franquicia,
            UltimosDigitos = ultimos4
        };
    }

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

    public Task<ResultadoPasarela> ProcesarDepositoAsync(decimal monto, string banco, string numeroCuenta, string titular)
    {
        var codigo = $"DEPOSITO-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
        var cuentaEnmascarada = numeroCuenta.Length > 4 ? $"***{numeroCuenta[^4..]}" : "***";

        return Task.FromResult(new ResultadoPasarela
        {
            CodigoTransaccion = codigo,
            Aprobado = true,
            Mensaje = $"Deposito de {monto:C} a {banco} cuenta {cuentaEnmascarada} a nombre de {titular}",
            Monto = monto
        });
    }

    public async Task<ResultadoPasarela> ConsultarEstadoAsync(string codigoTransaccion)
    {
        await Task.Delay(200);

        return new ResultadoPasarela
        {
            CodigoTransaccion = codigoTransaccion,
            Aprobado = true,
            Mensaje = "Transaccion confirmada",
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
    public string Franquicia { get; set; } = string.Empty;
    public string UltimosDigitos { get; set; } = string.Empty;
    public bool RequiereCuotas { get; set; }
}
