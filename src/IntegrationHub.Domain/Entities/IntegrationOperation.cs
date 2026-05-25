namespace IntegrationHub.Domain.Entities;

/// <summary>Representa una operación de integración transaccional.</summary>
public class IntegrationOperation
{
    /// <summary>Identificador único de la operación.</summary>
    public Guid Id { get; set; }

    /// <summary>Identificador de referencia en el sistema externo.</summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>Contenido de la operación en formato JSON.</summary>
    public string Payload { get; set; } = string.Empty;

    /// <summary>Nombre del sistema destino (ej. REST_SYSTEM_A, SOAP_SYSTEM_B).</summary>
    public string DestinationSystem { get; set; } = string.Empty;

    /// <summary>Estado actual de la operación (Pending, Completed, Failed).</summary>
    public string Status { get; set; } = "Pending";

    /// <summary>Fecha de creación del registro.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha en la que se procesó la operación.</summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>Mensaje de error en caso de fallo.</summary>
    public string? ErrorMessage { get; set; }
}
