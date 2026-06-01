namespace SistemaSubastaBackend.DTOs;

public class PagoTarjetaDTO
{
    public int SubastaId { get; set; }
    public int UsuarioId { get; set; }
    public decimal Monto { get; set; }
    public string NumeroTarjeta { get; set; } = string.Empty;
    public string FechaExpiracion { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
    public string NombreTitular { get; set; } = string.Empty;
}
