using RSAM.Domain.Entities.Document.ValueObjects;
using RSAM.Domain.SharedModels;

namespace RSAM.Domain.Entities.Document;

public sealed class Document : Entity<DocumentId>
{
    public string Name { get; private set; }
    public string ContentType { get; private set; }
    public long Size { get; private set; }
    public string StorageKey { get; private set; }
    public string? Description { get; private set; }
#pragma warning disable CS8618
    private Document() { }
#pragma warning restore CS8618
    private Document(DocumentId id, string name, string contentType, long size, string storageKey, string? description) : base(id)
    {
        Name = name;
        ContentType = contentType;
        Size = size;
        StorageKey = storageKey;
        Description = description;
    }
    internal static Document Create(string name, string contentType, long size, string storageKey, string? description)
    {
        return new Document(DocumentId.CreateUnique(), name, contentType, size, storageKey, description);
    }

}
