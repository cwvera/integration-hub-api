using Microsoft.EntityFrameworkCore;
using IntegrationHub.Domain.Entities;
using IntegrationHub.Application.Operations.Services;

namespace IntegrationHub.Infrastructure.Persistence;

/// <summary>Contexto de base de datos para la gestión de operaciones de integración.</summary>
public class IntegrationHubDbContext : DbContext, IApplicationDbContext
{
    /// <summary>Inicializa una nueva instancia del contexto.</summary>
    /// <param name="options">Opciones de configuración del contexto.</param>
    public IntegrationHubDbContext(DbContextOptions<IntegrationHubDbContext> options) : base(options) { }

    /// <summary>Colección de operaciones de integración.</summary>
    public DbSet<IntegrationOperation> Operations => Set<IntegrationOperation>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IntegrationOperation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ExternalId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
        });
    }

    /// <inheritdoc/>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
