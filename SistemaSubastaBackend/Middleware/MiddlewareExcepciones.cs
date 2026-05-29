using System.Net;
using System.Text.Json;
using SistemaSubastaBackend.Utilidades;

namespace SistemaSubastaBackend.Middleware;

public class MiddlewareExcepciones
{
    private readonly RequestDelegate _siguiente;

    public MiddlewareExcepciones(RequestDelegate siguiente)
    {
        _siguiente = siguiente;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _siguiente(contexto);
        }
        catch (Exception ex)
        {
            await ManejarExcepcionAsync(contexto, ex);
        }
    }

    private static async Task ManejarExcepcionAsync(HttpContext contexto, Exception ex)
    {
        contexto.Response.ContentType = "application/json";

        (int codigo, string mensaje) = ex switch
        {
            KeyNotFoundException => (404, ex.Message),
            UnauthorizedAccessException => (401, ex.Message),
            ArgumentException => (400, ex.Message),
            InvalidOperationException => (400, ex.Message),
            _ => (500, "Error interno del servidor")
        };

        contexto.Response.StatusCode = codigo;

        var respuesta = AyudanteRespuestaAPI.RespuestaError(mensaje, codigo);
        var json = JsonSerializer.Serialize(respuesta);

        await contexto.Response.WriteAsync(json);
    }
}
