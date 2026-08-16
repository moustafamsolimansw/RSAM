using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.Enums;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Application.Repositories;

public interface IWriteUserRepositry : IWriteRepository<User, UserId>
{
    Task UpdateUserOtp(UserId userId, string hashedOtp, OTPPurpose oTPPurpose, OTPChannel oTPChannel, CancellationToken cancellationToken);
}
