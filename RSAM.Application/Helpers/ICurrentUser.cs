namespace RSAM.Application.Helpers;

public interface ICurrentUser
{
    Guid? UserId { get; }
    string? UserName { get; }
    string? FullName { get; }
    bool ? IsAuthenticated { get; }
    bool? IsInRole(string role);
    bool? HasPermission (string permission);
}
