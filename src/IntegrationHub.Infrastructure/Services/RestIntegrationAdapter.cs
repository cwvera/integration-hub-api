using IntegrationHub.Application.Operations.Services;
using IntegrationHub.Domain.Entities;

namespace IntegrationHub.Infrastructure.Services;

/// <summary>Adaptador para integraciones basadas en REST.</summary>
public class RestIntegrationAdapter : IIntegrationAdapter
{
    private readonly HttpClient _httpClient;
    
    /// <inheritdoc/>
    public string SystemName { get; }

    /// <summary>Inicializa una nueva instancia del adaptador REST.</summary>
    /// <param name="httpClient">Cliente HTTP configurado.</param>
    /// <param name="systemName">Nombre del sistema destino.</param>
    public RestIntegrationAdapter(HttpClient httpClient, string systemName)
    {
        _httpClient = httpClient;
        SystemName = systemName;
    }

    /// <inheritdoc/>
    public async Task ProcessAsync(IntegrationOperation operation, CancellationToken ct)
    {
        await Task.Delay(50, ct); 
    }
}
