using RSAM.Domain.SharedModels;
using RSAM.Domain.UserAR.Enums;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Domain.UserAR.Entities;

public class UserOTP : BaseEntity<UserOTPId>
{
    public UserId UserId { get; private set; }
    public OTPPurpose Purpose { get; private set; }

    public OTPChannel Channel { get; private set; }
    public string HashedOTP { get; private set; }
    public DateTime ExpireAt { get; private set; }
    public bool IsUsed { get; private set; }

#pragma warning disable CS8618
    private UserOTP() { }
#pragma warning restore CS8618
    private UserOTP(UserOTPId id, UserId userId, OTPPurpose purpose, OTPChannel channel, string hashedOTP, DateTime expireAt) :base(id)
    {
        UserId = userId;
        Purpose = purpose;
        Channel = channel;
        HashedOTP = hashedOTP;
        ExpireAt = expireAt;
        IsUsed = false;
    }
    public static UserOTP Create(UserId userId, OTPPurpose purpose, OTPChannel channel, string hashedOTP, DateTime expireAt)
    {
        return new UserOTP(UserOTPId.CreateUnique(), userId, purpose, channel, hashedOTP, expireAt);
    }
    public void MarkAsUsed() => IsUsed = true;
}
