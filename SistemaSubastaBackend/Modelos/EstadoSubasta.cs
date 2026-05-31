namespace SistemaSubastaBackend.Modelos;

public enum EstadoSubasta
{
    Activa,
    PendientePago,
    Vendida,
    Incumplida,
    Cancelada
}

public static class EstadoSubastaHelper
{
    public static string AString(this EstadoSubasta estado) => estado switch
    {
        EstadoSubasta.Activa => "activa",
        EstadoSubasta.PendientePago => "pendiente_pago",
        EstadoSubasta.Vendida => "vendida",
        EstadoSubasta.Incumplida => "incumplida",
        EstadoSubasta.Cancelada => "cancelada",
        _ => "desconocida"
    };
}
