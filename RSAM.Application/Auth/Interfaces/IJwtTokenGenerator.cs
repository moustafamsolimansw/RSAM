using RSAM.Domain.UserAR;

namespace RSAM.Application.Auth.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
