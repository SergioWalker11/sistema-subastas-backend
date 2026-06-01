namespace SistemaSubastaBackend.DTOs;

public class PagoRespuestaDTO
{
    public int Id { get; set; }
    public int SubastaId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public string CodigoTransaccion { get; set; } = string.Empty;
    public string EstadoPago { get; set; } = string.Empty;
    public DateTime FechaPago { get; set; }
    public string? Franquicia { get; set; }
    public string? UltimosDigitos { get; set; }
}
