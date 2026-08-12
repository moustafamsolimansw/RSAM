using RSAM.Domain.SharedModels;
using RSAM.Domain.ValueObjects;

namespace RSAM.Domain.UserAR.Events;

public class UpdateUserPersonalInfoDomainEvent : IDomainEvent
{
    public Guid EventId { get; private set; }

    public DateTime OccuredOn { get; private set; }
    public Guid UserId { get; private set; }
    public string UserName { get; private set; }
    public PersonInfo NewPersonalInfo { get; private set; }

#pragma warning disable CS8618
    private UpdateUserPersonalInfoDomainEvent() { }
#pragma warning restore CS8618
    private UpdateUserPersonalInfoDomainEvent(Guid userId, string userName, PersonInfo newPersonalInfo)
    {
        EventId = Guid.CreateVersion7();
        OccuredOn = DateTime.UtcNow;
        UserId = userId;
        UserName = userName;
        NewPersonalInfo = newPersonalInfo;
    }
    public static UpdateUserPersonalInfoDomainEvent Create(Guid userId, string userName, PersonInfo newPersonalInfo)
        => new UpdateUserPersonalInfoDomainEvent(userId, userName, newPersonalInfo);
}
