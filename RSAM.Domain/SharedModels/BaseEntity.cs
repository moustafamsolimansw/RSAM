namespace RSAM.Domain.SharedModels;

public abstract class BaseEntity<TId> : IHasDomainEvent, IEquatable<BaseEntity<TId>> where TId : notnull
{
    public TId Id { get; private set; }
    public string? Code { get; private set; }
    public DateTime CreatedAt { get; protected set; }
    public string CreatedBy { get; protected set; } = string.Empty;

    public DateTime? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    public DateTime? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; }

    public bool IsDeleted { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
#pragma warning disable CS8618
    protected BaseEntity() { }
#pragma warning restore CS8618
    protected BaseEntity(TId id, string createdBy = "")
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }
    public void Update(string updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
    public void SoftDelete(string deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }

    public override bool Equals(object? obj)
    {
        return obj is BaseEntity<TId> entity && Id.Equals(entity.Id);
    }

    public bool Equals(BaseEntity<TId>? other)
    {
        return Equals((object?)other);
    }

    public static bool operator ==(BaseEntity<TId> left, BaseEntity<TId> right)
        => Equals(left, right);
    public static bool operator !=(BaseEntity<TId> left, BaseEntity<TId> right)
        => !Equals(left, right);
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    protected void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents()
        => _domainEvents.Clear();

   
}
