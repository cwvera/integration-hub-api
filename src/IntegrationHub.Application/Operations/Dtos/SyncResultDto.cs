namespace IntegrationHub.Application.Operations.Dtos;

/// <summary>Representa el resumen de los resultados obtenidos tras el proceso de sincronización.</summary>
/// <param name="ProcessedCount">Cantidad de operaciones procesadas exitosamente.</param>
/// <param name="FailedCount">Cantidad de operaciones que fallaron durante el procesamiento.</param>
/// <param name="Status">Estado final descriptivo del proceso.</param>
public record SyncResultDto(int ProcessedCount, int FailedCount, string Status);
