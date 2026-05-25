using FluentAssertions;
using IntegrationHub.Application.Operations.Commands;
using IntegrationHub.Application.Operations.Handlers;
using IntegrationHub.Application.Operations.Services;
using IntegrationHub.Domain.Entities;
using IntegrationHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IntegrationHub.Test;

/// <summary>Pruebas unitarias para el handler de sincronización de operaciones.</summary>
public class SyncOperationsHandlerTests
{
    private readonly IntegrationHubDbContext _db;
    private readonly Mock<IIntegrationAdapter> _adapterMock;
    private readonly IntegrationAdapterFactory _adapterFactory;
    private readonly Mock<ILogger<SyncOperationsHandler>> _loggerMock;
    private readonly SyncOperationsHandler _handler;

    public SyncOperationsHandlerTests()
    {
        var options = new DbContextOptionsBuilder<IntegrationHubDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _db = new IntegrationHubDbContext(options);
        
        _adapterMock = new Mock<IIntegrationAdapter>();
        _adapterMock.Setup(a => a.SystemName).Returns("TEST_SYSTEM");
        
        _adapterFactory = new IntegrationAdapterFactory(new[] { _adapterMock.Object });
        
        _loggerMock = new Mock<ILogger<SyncOperationsHandler>>();
        _handler = new SyncOperationsHandler(_db, _adapterFactory, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnNoPendingMessage_WhenNoPendingOperationsExist()
    {
        var command = new SyncOperationsCommand();
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Message.Should().Be("No hay operaciones pendientes");
        result.Data!.ProcessedCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldProcessPendingOperations_WhenTheyExist()
    {
        var operations = new List<IntegrationOperation>
        {
            new() { Id = Guid.NewGuid(), ExternalId = "EXT1", Status = "Pending", DestinationSystem = "TEST_SYSTEM" },
            new() { Id = Guid.NewGuid(), ExternalId = "EXT2", Status = "Pending", DestinationSystem = "TEST_SYSTEM" }
        };
        await _db.Operations.AddRangeAsync(operations);
        await _db.SaveChangesAsync();

        var command = new SyncOperationsCommand(BatchSize: 10);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data!.ProcessedCount.Should().Be(2);
        _adapterMock.Verify(c => c.ProcessAsync(It.IsAny<IntegrationOperation>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        
        var updatedOperations = await _db.Operations.ToListAsync();
        updatedOperations.Should().AllSatisfy(o => o.Status.Should().Be("Completed"));
    }

    [Fact]
    public async Task Handle_ShouldHandleFailures_WhenAdapterThrowsException()
    {
        var operation = new IntegrationOperation { Id = Guid.NewGuid(), ExternalId = "EXT1", Status = "Pending", DestinationSystem = "TEST_SYSTEM" };
        await _db.Operations.AddAsync(operation);
        await _db.SaveChangesAsync();

        _adapterMock.Setup(c => c.ProcessAsync(It.IsAny<IntegrationOperation>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("External Error"));

        var command = new SyncOperationsCommand();
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Data!.FailedCount.Should().Be(1);
        result.Data.ProcessedCount.Should().Be(0);
        
        var updatedOperation = await _db.Operations.FirstAsync();
        updatedOperation.Status.Should().Be("Failed");
        updatedOperation.ErrorMessage.Should().Be("External Error");
    }
}
