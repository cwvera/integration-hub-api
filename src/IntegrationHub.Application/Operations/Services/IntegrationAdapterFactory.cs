namespace IntegrationHub.Application.Operations.Services;

/// <summary>Factoría para resolver el adaptador de integración adecuado según el sistema destino.</summary>
public class IntegrationAdapterFactory
{
    private readonly IEnumerable<IIntegrationAdapter> _adapters;

    /// <summary>Inicializa una nueva instancia de la factoría.</summary>
    /// <param name="adapters">Lista de adaptadores registrados.</param>
    public IntegrationAdapterFactory(IEnumerable<IIntegrationAdapter> adapters)
    {
        _adapters = adapters;
    }

    /// <summary>Obtiene el adaptador correspondiente para un sistema específico.</summary>
    /// <param name="systemName">Nombre del sistema externo.</param>
    /// <returns>El adaptador de integración.</returns>
    /// <exception cref="NotSupportedException">Si el sistema no está soportado.</exception>
    public IIntegrationAdapter GetAdapter(string systemName)
    {
        return _adapters.FirstOrDefault(a => a.SystemName.Equals(systemName, StringComparison.OrdinalIgnoreCase))
               ?? throw new NotSupportedException($"El sistema '{systemName}' no tiene un adaptador configurado.");
    }
}
