namespace RSAM.Domain.SharedModels;

public abstract class ValueObject
{
    public abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if(obj is null || obj.GetType() != GetType())
            return false;
        var valueObject = obj as ValueObject;
        if (valueObject is null) return false;
        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }
    public override int GetHashCode()
    {
        return GetEqualityComponents().Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
    public static bool operator ==(ValueObject? a, ValueObject? b)
     => Equals(a, b);
    public static bool operator !=(ValueObject? a, ValueObject? b)
     => !Equals(a, b);
    public override string ToString()
    {
        return string.Join(", ", GetEqualityComponents());
    }
    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }
}
