using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IntegrationHub.Application.Operations.Commands;
using IntegrationHub.Application.Operations.Dtos;
using IntegrationHub.Commons.Responses;

namespace IntegrationHub.WebApi.Controllers.Version1;

/// <summary>Controlador para la gestión de procesos de sincronización e integración.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SyncController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>Inicializa una nueva instancia del controlador.</summary>
    /// <param name="mediator">Mediador para el despacho de comandos y consultas.</param>
    public SyncController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Dispara el proceso de sincronización masiva de operaciones pendientes.</summary>
    /// <param name="command">Comando con los parámetros de sincronización.</param>
    /// <returns>Resultado del proceso de sincronización.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CommandResponse<SyncResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SyncAsync([FromBody] SyncOperationsCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
