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

    public PhoneNumber PhoneNumber { get; private set; }
    private readonly List<UserCredentials> _userCredentials = new();
    public IReadOnlyCollection<UserCredentials> UserCredentials => _userCredentials.AsReadOnly();

    private readonly List<UserOTP> _userOTPs = new();
    public IReadOnlyCollection<UserOTP> UserOTPs => _userOTPs.AsReadOnly();
#pragma warning disable CS8618
    private User() { }
#pragma warning restore CS8618
    private User(UserId id, string username, EmailAddress emailAddress, PersonInfo personInfo, PhoneNumber phoneNumber) : base(id)
    {
        Username = username;
        EmailAddress = emailAddress;
        PersonInfo = personInfo;
        PhoneNumber = phoneNumber;
        this.AddDomainEvent(CreateUserDomainEvent.Create(id.Value, username));
    }
    public static User Create(string username, EmailAddress emailAddress, PersonInfo personInfo, PhoneNumber phoneNumber)
    {
        return new User(UserId.CreateUnique(), username, emailAddress, personInfo, phoneNumber);
    }
    public void UpdatePersonalInfo(PersonInfo personInfo, string updatedBy)
    {
        PersonInfo = personInfo;
        this.AddDomainEvent(UpdateUserPersonalInfoDomainEvent.Create(this.Id.Value, this.Username, personInfo));
        Update(updatedBy);
    }
    public void AddUserCredential(UserCredentials userCredentials, string updatedBy)
    {
        _userCredentials.Add(userCredentials);
        this.AddDomainEvent(AddUserCredentialDomainEvent.Create(this.Id.Value, this.Username));
        Update(updatedBy);
    }
    public void AddUserOTP(UserOTP userOTP, string updatedBy)
    {
        _userOTPs.Add(userOTP);
        this.AddDomainEvent(AddUserOTPDomainEvent.Create(this.Id.Value, this.Username, userOTP.Purpose, userOTP.Channel));
        Update(updatedBy);
    }
    
}
