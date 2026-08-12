using RSAM.Domain.SharedModels;

namespace RSAM.Application.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
