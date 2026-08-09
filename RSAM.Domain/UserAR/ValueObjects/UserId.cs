using RSAM.Domain.SharedModels;
using System.ComponentModel.Design;

namespace RSAM.Domain.UserAR.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; private set; }
#pragma warning disable CS8618
    private UserId() { }
#pragma warning restore CS8618
    private UserId(Guid value) => Value = value;
    public static UserId CreateUnique() => new(Guid.CreateVersion7());
    public static UserId Create(Guid value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
