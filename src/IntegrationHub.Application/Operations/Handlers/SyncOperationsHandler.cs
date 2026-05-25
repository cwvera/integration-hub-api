using MediatR;
using Microsoft.EntityFrameworkCore;
using IntegrationHub.Application.Operations.Commands;
using IntegrationHub.Application.Operations.Dtos;
using IntegrationHub.Application.Operations.Services;
using IntegrationHub.Commons.Responses;
using Microsoft.Extensions.Logging;

namespace IntegrationHub.Application.Operations.Handlers;

/// <summary>Handler que orquesta la sincronización paralela de operaciones hacia múltiples sistemas.</summary>
public class SyncOperationsHandler : IRequestHandler<SyncOperationsCommand, CommandResponse<SyncResultDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IntegrationAdapterFactory _adapterFactory;
    private readonly ILogger<SyncOperationsHandler> _logger;

    public SyncOperationsHandler(IApplicationDbContext db, IntegrationAdapterFactory adapterFactory, ILogger<SyncOperationsHandler> logger)
    {
        _db = db;
        _adapterFactory = adapterFactory;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CommandResponse<SyncResultDto>> Handle(SyncOperationsCommand request, CancellationToken cancellationToken)
    {
        var pendingOperations = await _db.Operations
            .Where(o => o.Status == "Pending")
            .Take(request.BatchSize)
            .ToListAsync(cancellationToken);

        if (!pendingOperations.Any())
        {
            return new CommandResponse<SyncResultDto> 
            { 
                Message = "No hay operaciones pendientes", 
                Data = new SyncResultDto(0, 0, "Idle") 
            };
        }

        int successCount = 0;
        int failCount = 0;

        await Parallel.ForEachAsync(pendingOperations, new ParallelOptions 
        { 
            MaxDegreeOfParallelism = 5,
            CancellationToken = cancellationToken 
        }, async (operation, ct) => 
        {
            try 
            {
                var adapter = _adapterFactory.GetAdapter(operation.DestinationSystem);
                await adapter.ProcessAsync(operation, ct);
                
                operation.Status = "Completed";
                operation.ProcessedAt = DateTime.UtcNow;
                Interlocked.Increment(ref successCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando operación {Id} para el sistema {System}", operation.Id, operation.DestinationSystem);
                operation.Status = "Failed";
                operation.ErrorMessage = ex.Message;
                Interlocked.Increment(ref failCount);
            }
        });

        await _db.SaveChangesAsync(cancellationToken);

        return new CommandResponse<SyncResultDto>
        {
            Message = $"Procesamiento finalizado: {successCount} exitosos, {failCount} fallidos",
            Data = new SyncResultDto(successCount, failCount, "Finished")
        };
    }
}
