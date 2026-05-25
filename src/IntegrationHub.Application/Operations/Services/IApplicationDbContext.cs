using IntegrationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IntegrationHub.Application.Operations.Services;

/// <summary>Define el contrato para el contexto de base de datos accesible desde la capa de aplicación.</summary>
public interface IApplicationDbContext
{
    /// <summary>Colección de operaciones de integración.</summary>
    DbSet<IntegrationOperation> Operations { get; }

    /// <summary>Guarda todos los cambios realizados en el contexto.</summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Tarea que representa la operación de guardado.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
