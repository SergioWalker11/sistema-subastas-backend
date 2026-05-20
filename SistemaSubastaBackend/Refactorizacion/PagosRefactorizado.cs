using System;
using System.Collections.Generic;

namespace SistemaSubastaBackend.Refactorizacion;

public class PagosRefactorizado
{
    // Constantes para eliminar numeros magicos
    private const decimal LimiteMaximoPago = 50000m;
    private const decimal LimiteReembolsoAutomatico = 1000m;
    private const decimal ComisionQR = 0.01m;
    private const decimal ComisionTransferencia = 0.02m;
    private const decimal ComisionTarjeta = 0.035m;
    private const decimal ComisionDefault = 0.05m;

    public ResultadoPago ProcesarPago(int subastaId, int usuarioId, decimal monto, string tipoPago, bool esReembolso)
    {
        // 1. Validacion separada
        if (!EsMontoValido(monto))
            return ResultadoPago.Fallo("Monto invalido o excede el limite de 50,000 Bs");

        // 2. Calculo separado
        decimal comision = CalcularComision(monto, tipoPago);
        decimal montoFinal = monto + comision;

        // 3. Logica de reembolso separada
        if (esReembolso)
            return ProcesarReembolso(montoFinal);

        // 4. Exito
        return ResultadoPago.Exitoso(GenerarCodigoTransaccion(), montoFinal, comision);
    }

    private bool EsMontoValido(decimal monto)
    {
        return monto > 0 && monto <= LimiteMaximoPago;
    }

    private decimal CalcularComision(decimal monto, string tipoPago)
    {
        return tipoPago switch
        {
            "qr" => monto * ComisionQR,
            "transferencia" => monto * ComisionTransferencia,
            "tarjeta" => monto * ComisionTarjeta,
            _ => monto * ComisionDefault
        };
    }

    private ResultadoPago ProcesarReembolso(decimal montoFinal)
    {
        if (montoFinal > LimiteReembolsoAutomatico)
            return ResultadoPago.Fallo("Reembolso manual requerido para montos mayores a 1,000 Bs");
        
        return ResultadoPago.Exitoso(GenerarCodigoTransaccion(), montoFinal, 0);
    }

    private string GenerarCodigoTransaccion()
    {
        return $"PAGO-{DateTime.Now:yyyyMMdd}-{new Random().Next(100, 999)}";
    }
}

// Clase simple para el resultado, sin exceso de propiedades
public class ResultadoPago
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public string? Codigo { get; set; }
    public decimal MontoFinal { get; set; }
    public decimal Comision { get; set; }

    public static ResultadoPago Exitoso(string codigo, decimal monto, decimal comision) => new()
    { Exito = true, Mensaje = "Operacion exitosa", Codigo = codigo, MontoFinal = monto, Comision = comision };

    public static ResultadoPago Fallo(string mensaje) => new() { Exito = false, Mensaje = mensaje };
}
