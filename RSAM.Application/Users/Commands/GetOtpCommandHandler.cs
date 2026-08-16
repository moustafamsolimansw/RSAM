using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using RSAM.Application.Auth.Interfaces;
using RSAM.Application.Helpers;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Common;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.Enums;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Application.Users.Commands;

public class GetOtpCommandHandler : IRequestHandler<GetOtpCommand, ErrorOr<bool>>
{
    private readonly IReadRepository<User, UserId> _userRepository;
    private readonly IWriteUserRepositry _writeUserRepository;
    private readonly IWriteUnitOfWork _writeUnitOfWork;
    private readonly IOtpGeneator _otpGenerator;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<GetOtpCommandHandler> _logger;

    public GetOtpCommandHandler(IReadRepository<User, UserId> userRepository, IWriteUserRepositry writeUserRepository , IWriteUnitOfWork writeUnitOfWork ,IOtpGeneator otpGenerator, ICurrentUser currentUser ,ILogger<GetOtpCommandHandler> logger)
    {
        _userRepository = userRepository;
        _writeUserRepository = writeUserRepository;
        _writeUnitOfWork = writeUnitOfWork;
        _otpGenerator = otpGenerator;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<ErrorOr<bool>> Handle(GetOtpCommand command, CancellationToken cancellationToken)
    {
        // Check if the user exists by username, email, or phone number
        var user = await _userRepository.GetFirstOrDefaultAsync(
            u => u.Username == command.usernameOrEmailOrPhoneNumber ||
                 u.EmailAddress.Value == command.usernameOrEmailOrPhoneNumber ||
                 u.PhoneNumber.Value == command.usernameOrEmailOrPhoneNumber,
            true,
            cancellationToken,
            u => u.UserCredentials, u=> u.UserOTPs
        );
        if(user is null)
        {
            return Error.Failure("User not found");
        }
        var existingOtp = user.UserOTPs.FirstOrDefault(opt => opt.ExpireAt > DateTime.UtcNow);
        if (existingOtp is not null) 
        {
            existingOtp.SoftDelete(_currentUser.FullName ?? "unknown");
        }
        var newOtp = await _otpGenerator.GenerateOtpAsync();
        if (newOtp is null) 
        {
            return Error.Failure("Can't generate otp, please try again later.");
        }
        await _writeUserRepository.UpdateUserOtp(user.Id, newOtp, OTPPurpose.ResetPassword, OTPChannel.Email, cancellationToken);
        await _writeUnitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
