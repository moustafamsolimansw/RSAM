using RSAM.Domain.SharedModels;
using RSAM.Domain.UserAR.ValueObjects;
using RSAM.Domain.ValueObjects;

namespace RSAM.Domain.UserAR;

public class User : AggregateRoot<UserId>
{
    public string Username { get; private set; }
    public EmailAddress EmailAddress { get; private set; }
    public PersonInfo PersonInfo { get; private set; }
#pragma warning disable CS8618
    private User() { }
#pragma warning restore CS8618
    private User(UserId id, string username, EmailAddress emailAddress, PersonInfo personInfo) : base(id)
    {
        Username = username;
        EmailAddress = emailAddress;
        PersonInfo = personInfo;
    }
}
