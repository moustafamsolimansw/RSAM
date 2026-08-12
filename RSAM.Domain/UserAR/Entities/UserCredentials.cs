using RSAM.Domain.SharedModels;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Domain.UserAR.Entities;

public class UserCredentials : BaseEntity<UserCredentialsId>
{
    public UserId UserId { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime? PasswordLastChangedAt { get; private set; }
#pragma warning disable CS8618
    private UserCredentials() { }
#pragma warning restore CS8618
    private UserCredentials(UserCredentialsId userCredentialsId, UserId userId, string passwordHash)
        :base(userCredentialsId)
    {
        UserId = userId;
        PasswordHash = passwordHash;
        PasswordLastChangedAt = DateTime.UtcNow;
    }
    public static UserCredentials Create(UserId userId, string passwordHash)
        => new (UserCredentialsId.CreateUnique(), userId, passwordHash);

}
