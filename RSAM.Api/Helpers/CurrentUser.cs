using RSAM.Application.Helpers;
using System.Security.Claims;

namespace RSAM.Api.Helpers;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    private ClaimsPrincipal? _principal => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var value = _principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(value, out var userId)
                ? userId
                : null;
        }
    }

    public string? UserName => _principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? FullName => _principal?.FindFirst(ClaimTypes.GivenName)?.Value;

    public bool? IsAuthenticated => _principal?.Identity?.IsAuthenticated == true;

    public bool? HasPermission(string permission) => _principal?.FindAll("permission").Any(x => x.Value == permission) == true;

    public bool? IsInRole(string role) => _principal?.IsInRole(role);
}
