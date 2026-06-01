using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.ServiciosExternos;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioPasarelaPagos
{
    Task<ResultadoPasarela> ProcesarPagoConTarjetaAsync(PagoTarjetaDTO dto);
    Task<ResultadoPasarela> ProcesarPagoAsync(decimal monto, string nombreUsuario, string correo);
    Task<ResultadoPasarela> ProcesarDepositoAsync(decimal monto, string banco, string numeroCuenta, string titular);
    Task<ResultadoPasarela> ConsultarEstadoAsync(string codigoTransaccion);
}
