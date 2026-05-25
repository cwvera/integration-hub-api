using IntegrationHub.Application.Operations.Services;
using IntegrationHub.Domain.Entities;

namespace IntegrationHub.Infrastructure.Services;

/// <summary>Adaptador para integraciones basadas en SOAP.</summary>
public class SoapIntegrationAdapter : IIntegrationAdapter
{
    /// <inheritdoc/>
    public string SystemName { get; }

    /// <summary>Inicializa una nueva instancia del adaptador SOAP.</summary>
    /// <param name="systemName">Nombre del sistema destino.</param>
    public SoapIntegrationAdapter(string systemName)
    {
        SystemName = systemName;
    }

    /// <inheritdoc/>
    public async Task ProcessAsync(IntegrationOperation operation, CancellationToken ct)
    {
        await Task.Delay(100, ct); 
    }
}
