using RSAM.Domain.SharedModels;
using RSAM.Domain.UserAR.ValueObjects;
using RSAM.Domain.ValueObjects;

namespace RSAM.Domain.UserAR;

public class User : AggregateRoot<UserId>
{
    public string Username { get; private set; }
    public PersonInfo PersonInfo { get; private set; }
#pragma warning disable CS8618
    private User() { }
#pragma warning restore CS8618
    
}
