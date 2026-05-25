using IntegrationHub.Domain.Entities;

namespace IntegrationHub.Application.Operations.Services;

/// <summary>Define el contrato para los adaptadores de integración con sistemas externos.</summary>
public interface IIntegrationAdapter
{
    /// <summary>Identificador único del sistema que maneja este adaptador.</summary>
    string SystemName { get; }

    /// <summary>Procesa una operación de integración de forma asíncrona.</summary>
    /// <param name="operation">La operación a procesar.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task ProcessAsync(IntegrationOperation operation, CancellationToken ct);
}
