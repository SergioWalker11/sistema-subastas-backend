using SistemaSubastaBackend.ServiciosExternos;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioPasarelaPagos
{
    Task<ResultadoPasarela> ProcesarPagoAsync(decimal monto, string nombreUsuario, string correo);
    Task<ResultadoPasarela> ConsultarEstadoAsync(string codigoTransaccion);
}
