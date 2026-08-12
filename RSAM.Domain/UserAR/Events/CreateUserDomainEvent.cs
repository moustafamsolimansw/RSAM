using RSAM.Domain.SharedModels;

namespace RSAM.Domain.UserAR.Events;

public class CreateUserDomainEvent : IDomainEvent
{
    public Guid EventId { get; private set; }

    public DateTime OccuredOn { get; private set; }
    public Guid UserId { get; private set; }
    public string UserName { get; private set; }

#pragma warning disable CS8618
    private CreateUserDomainEvent() { }
#pragma warning restore CS8618
    private CreateUserDomainEvent(Guid userId, string userName)
    {
        EventId = Guid.CreateVersion7();
        OccuredOn = DateTime.UtcNow;
        UserId = userId;
        UserName = userName;
    }
    public static CreateUserDomainEvent Create(Guid userId, string userName)
        => new CreateUserDomainEvent(userId, userName);
}
