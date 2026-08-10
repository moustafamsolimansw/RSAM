using RSAM.Domain.SharedModels;

namespace RSAM.Domain.Entities.Document.ValueObjects;

public class DocumentId : ValueObject
{
    public Guid Value { get; private set; }
#pragma warning disable CS8618
    private DocumentId() { }
#pragma warning restore CS8618
    private DocumentId(Guid value)
    {
        Value = value;
    }
    public static DocumentId CreateUnique()
    {
        return new DocumentId(Guid.CreateVersion7());
    }
    public static DocumentId Create(Guid value)
    {
        return new DocumentId(value);
    }
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString()
        => Value.ToString();
}
