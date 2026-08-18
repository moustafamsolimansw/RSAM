using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RSAM.Application.Helpers;
using RSAM.Application.Repositories;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.Entities;
using RSAM.Domain.UserAR.Enums;
using RSAM.Domain.UserAR.ValueObjects;
using RSAM.Domain.ValueObjects;
using RSAM.Infrastructure.Auth;
using RSAM.Infrastructure.Context;

namespace RSAM.Infrastructure.Repositories;

public class WriteUserRepository : WriteRepository<User, UserId> ,IWriteUserRepositry
{

    private readonly DbSet<User>   _users;
    private readonly IReadRepository<User, UserId> _usersReadRepo;
    private readonly ICurrentUser  _currentUser;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<WriteUserRepository> _logger;

    public WriteUserRepository(RSAMDbContext context, IReadRepository<User, UserId> usersReadRepo, ICurrentUser currentUser, IOptions<JwtSettings> jwtSettings,ILogger<WriteUserRepository> logger)
        :base(context)
    {
        _users = context.Set<User>();
        _usersReadRepo = usersReadRepo;
        _currentUser = currentUser;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task UpdateUserOtp(UserId userId, string hashedOtp, OTPPurpose oTPPurpose, OTPChannel oTPChannel, CancellationToken cancellationToken)
    {
        if (_currentUser is null)
        {
            _logger.LogError("Please login to complete this update process");
            return;
        }

        try {
            var user = await _usersReadRepo.GetFirstOrDefaultAsync(u => u.Id == userId, false, cancellationToken ,u=>u.UserOTPs);
            if (user is null)
                throw new Exception("Can NOT find the user");
            var activeOtps = user.UserOTPs.Where(otp=> otp.ExpireAt > DateTime.UtcNow && otp.IsUsed == false).ToList();
            if (activeOtps is not null && activeOtps.Count > 0)
            {
                foreach (var otp in activeOtps) 
                {

                    otp.MarkAsUsed();
                }
            }
            user.AddUserOTP(UserOTP.Create(userId, oTPPurpose, oTPChannel, hashedOtp, DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiration)), _currentUser.FullName ?? "unknown");
            _users.Update(user);
            await Task.CompletedTask;
        }
        catch (Exception ex) {
            _logger.LogError(ex.Message, ex.GetBaseException());
        }
        
    }

    public async Task UpdateUserPhoneNumber(User user, string phoneNumber, CancellationToken cancellationToken)
    {
        user.UpdatePhoneNumber(PhoneNumber.Create(phoneNumber));

    }
}
