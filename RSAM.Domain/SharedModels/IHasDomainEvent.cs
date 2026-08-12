namespace RSAM.Domain.SharedModels;

public interface IHasDomainEvent
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
