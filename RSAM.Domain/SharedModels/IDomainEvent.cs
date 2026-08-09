namespace RSAM.Domain.SharedModels;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccuredOn { get; }
}
