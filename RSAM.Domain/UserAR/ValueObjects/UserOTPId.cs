using RSAM.Domain.SharedModels;

namespace RSAM.Domain.UserAR.ValueObjects;

public class UserOTPId : ValueObject
{
    public Guid Value { get; private set; }
#pragma warning disable CS8618
    private UserOTPId() { }
#pragma warning restore CS8618
    private UserOTPId(Guid value) => Value = value;
    public static UserOTPId CreateUnique() => new(Guid.CreateVersion7());
    public static UserOTPId Create(Guid value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
