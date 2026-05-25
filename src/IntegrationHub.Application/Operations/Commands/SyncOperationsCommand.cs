using MediatR;
using IntegrationHub.Application.Operations.Dtos;
using IntegrationHub.Commons.Responses;

namespace IntegrationHub.Application.Operations.Commands;

/// <summary>Comando para iniciar el proceso de sincronización masiva de operaciones.</summary>
/// <param name="BatchSize">Cantidad de registros a procesar en un solo lote.</param>
public record SyncOperationsCommand(int BatchSize = 100) : IRequest<CommandResponse<SyncResultDto>>;
