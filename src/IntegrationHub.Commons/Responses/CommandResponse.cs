namespace IntegrationHub.Commons.Responses;

/// <summary>Respuesta estandarizada para todos los comandos de la aplicación.</summary>
/// <typeparam name="T">Tipo de los datos retornados.</typeparam>
public class CommandResponse<T>
{
    /// <summary>Mensaje informativo sobre el resultado de la operación.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>Datos resultantes de la operación.</summary>
    public T? Data { get; set; }

    /// <summary>Indica si la operación fue exitosa.</summary>
    public bool Success { get; set; } = true;
}
