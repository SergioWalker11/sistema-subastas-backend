namespace SistemaSubastaBackend.DTOs;

public class DatosBancariosDTO
{
    public string Banco { get; set; } = string.Empty;
    public string TipoCuenta { get; set; } = "ahorros";
    public string NumeroCuenta { get; set; } = string.Empty;
    public string Titular { get; set; } = string.Empty;
}
