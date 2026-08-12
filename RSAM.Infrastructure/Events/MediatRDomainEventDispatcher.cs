using MediatR;
using Microsoft.Extensions.Logging;
using RSAM.Application.Events;
using RSAM.Domain.SharedModels;

namespace RSAM.Infrastructure.Events;

public class MediatRDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _publisher;
    private readonly ILogger<MediatRDomainEventDispatcher> _logger;

    public MediatRDomainEventDispatcher(IPublisher publisher, ILogger<MediatRDomainEventDispatcher> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Dispatching domain event: {EventId} of type {EventType}", domainEvent.EventId, domainEvent.GetType().Name);
        await _publisher.Publish(domainEvent, cancellationToken);
        _logger.LogInformation("Domain event dispatched: {EventId} of type {EventType}", domainEvent.EventId, domainEvent.GetType().Name);
    }
}
