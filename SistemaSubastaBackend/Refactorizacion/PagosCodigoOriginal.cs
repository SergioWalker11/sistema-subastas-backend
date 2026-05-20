using System;
using System.Collections.Generic;

namespace SistemaSubastaBackend.Refactorizacion;

public class PagosCodigoOriginal
{
    // Método gigante con toda la lógica mezclada
    public Dictionary<string, object> ProcesarPago(int subastaId, int usuarioId, decimal monto, string tipoPago, bool esReembolso)
    {
        var respuesta = new Dictionary<string, object>();
        
        // Validación mezclada con lógica
        if (monto <= 0) { respuesta.Add("exito", false); respuesta.Add("mensaje", "Monto invalido"); return respuesta; }
        if (monto > 50000) { respuesta.Add("exito", false); respuesta.Add("mensaje", "Monto excede limite"); return respuesta; }

        decimal comision = 0;
        // Numeros magicos y switch gigante
        if (tipoPago == "qr") { comision = monto * 0.01m; } // 1%
        else if (tipoPago == "transferencia") { comision = monto * 0.02m; } // 2%
        else if (tipoPago == "tarjeta") { comision = monto * 0.035m; } // 3.5%
        else { comision = monto * 0.05m; } // 5% default

        decimal montoFinal = monto + comision;

        // Logica de reembolso mezclada aqui
        if (esReembolso)
        {
            if (montoFinal > 1000)
            {
                respuesta.Add("exito", false);
                respuesta.Add("mensaje", "Reembolso manual requerido para montos altos");
                return respuesta;
            }
            respuesta.Add("exito", true);
            respuesta.Add("mensaje", "Reembolso procesado");
            respuesta.Add("monto", montoFinal);
            return respuesta;
        }

        // Generacion de codigo duplicada
        string codigo = "PAGO-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(100, 999);

        respuesta.Add("exito", true);
        respuesta.Add("mensaje", "Pago exitoso");
        respuesta.Add("codigo", codigo);
        respuesta.Add("montoFinal", montoFinal);
        respuesta.Add("comision", comision);
        
        return respuesta;
    }
}
