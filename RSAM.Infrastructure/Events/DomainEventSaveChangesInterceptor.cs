using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using RSAM.Application.Events;
using RSAM.Domain.SharedModels;

namespace RSAM.Infrastructure.Events;

public class DomainEventSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ILogger<DomainEventSaveChangesInterceptor> _logger;

    public DomainEventSaveChangesInterceptor(IDomainEventDispatcher domainEventDispatcher, ILogger<DomainEventSaveChangesInterceptor> logger)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _logger = logger;
    }

    public override async ValueTask<InterceptionResult<int>>
        SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return result;

        var entities = context
            .ChangeTracker
            .Entries<IHasDomainEvent>()
            .Select(x => x.Entity)
            .ToList();

        var events = entities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        foreach (var @event in events)
        {
            await _domainEventDispatcher.DispatchAsync(
                @event,
                cancellationToken);
        }

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }

        return result;
    }

}
