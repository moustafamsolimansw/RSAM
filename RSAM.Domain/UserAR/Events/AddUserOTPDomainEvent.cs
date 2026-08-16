using RSAM.Domain.SharedModels;
using RSAM.Domain.UserAR.Enums;

namespace RSAM.Domain.UserAR.Events;

public class AddUserOTPDomainEvent : IDomainEvent

{
    public Guid EventId { get; private set; }
    public DateTime OccuredOn { get; private set; }
    public Guid UserId { get; private set; }
    public string UserName { get; private set; }
    public OTPPurpose Purpose { get; private set; }
    public OTPChannel Channel { get; private set; }
#pragma warning disable CS8618
    private AddUserOTPDomainEvent() { }
#pragma warning restore CS8618
    private AddUserOTPDomainEvent(Guid userId, string userName, OTPPurpose purpose, OTPChannel channel)
    {
        EventId = Guid.CreateVersion7();
        OccuredOn = DateTime.UtcNow;
        UserId = userId;
        UserName = userName;
        Purpose = purpose;
        Channel = channel;
    }
    public static AddUserOTPDomainEvent Create(Guid userId, string userName, OTPPurpose purpose, OTPChannel channel)
    {
        return new AddUserOTPDomainEvent(userId, userName, purpose, channel);
    }
}
