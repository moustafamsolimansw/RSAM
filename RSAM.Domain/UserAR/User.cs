using RSAM.Domain.SharedModels;
using RSAM.Domain.UserAR.Entities;
using RSAM.Domain.UserAR.Events;
using RSAM.Domain.UserAR.ValueObjects;
using RSAM.Domain.ValueObjects;

namespace RSAM.Domain.UserAR;

public class User : AggregateRoot<UserId>
{
    public string Username { get; private set; }
    public EmailAddress EmailAddress { get; private set; }
    public PersonInfo PersonInfo { get; private set; }
    private readonly List<UserCredentials> _userCredentials = new();
    public IReadOnlyCollection<UserCredentials> UserCredentials => _userCredentials.AsReadOnly();
#pragma warning disable CS8618
    private User() { }
#pragma warning restore CS8618
    private User(UserId id, string username, EmailAddress emailAddress, PersonInfo personInfo) : base(id)
    {
        Username = username;
        EmailAddress = emailAddress;
        PersonInfo = personInfo;
        this.AddDomainEvent(CreateUserDomainEvent.Create(id.Value, username));
    }
    public static User Create(string username, EmailAddress emailAddress, PersonInfo personInfo)
    {
        return new User(UserId.CreateUnique(), username, emailAddress, personInfo);
    }
    public void UpdatePersonalInfo(PersonInfo personInfo, string updatedBy)
    {
        PersonInfo = personInfo;
        this.AddDomainEvent(UpdateUserPersonalInfoDomainEvent.Create(this.Id.Value, this.Username, personInfo));
        Update(updatedBy);
    }
}
