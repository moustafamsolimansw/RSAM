using RSAM.Domain.SharedModels;

namespace RSAM.Domain.UserAR.ValueObjects;

public class UserCredentialsId : ValueObject
{
    public Guid Value { get; private set; }
#pragma warning disable CS8618
    private UserCredentialsId() { }
#pragma warning restore CS8618
    private UserCredentialsId(Guid value)
    {
        Value = value;
    }
    public static UserCredentialsId CreateUnique()
        => new (Guid.CreateVersion7());
    public static UserCredentialsId Create(Guid value)
        => new (value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
