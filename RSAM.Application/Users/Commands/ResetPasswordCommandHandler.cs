using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using RSAM.Application.Auth.Interfaces;
using RSAM.Application.Helpers;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Common;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.Entities;
using RSAM.Domain.UserAR.ValueObjects;
using static System.Net.WebRequestMethods;

namespace RSAM.Application.Users.Commands;

public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<AuthResult>>
{
    private readonly IReadRepository<User, UserId> _usersReadRepo;
    private readonly IWriteUserRepositry _writeUserRepositry;
    private readonly IWriteRepository<UserCredentials, UserCredentialsId> _credentialsWriteRepo;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICurrentUser _currentUser;
    private readonly IWriteUnitOfWork _writeUnitOfWork;

    public ResetPasswordCommandHandler(IReadRepository<User, UserId> usersReadRepo, IWriteUserRepositry writeUserRepositry,
        IWriteRepository<UserCredentials, UserCredentialsId> credentialsWriteRepo,
        ILogger<ResetPasswordCommandHandler> logger, IJwtTokenGenerator jwtTokenGenerator, ICurrentUser currentUser, IWriteUnitOfWork writeUnitOfWork)
    {
        _usersReadRepo = usersReadRepo;
        _writeUserRepositry = writeUserRepositry;
        _credentialsWriteRepo = credentialsWriteRepo;
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator;
        _currentUser = currentUser;
        _writeUnitOfWork = writeUnitOfWork;
    }

    public async Task<ErrorOr<AuthResult>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        if (command.newPassword != command.confirmNewPassword)
        {
            _logger.LogError("New password is NOT match the confirm one");
            return ErrorOr.Error.Failure("New password is NOT match the confirm one");
        }
        try
        {
            var user = await _usersReadRepo.GetFirstOrDefaultAsync(
            u => u.EmailAddress.Value == command.userNameEmailOrPhoneNumber || u.Username == command.userNameEmailOrPhoneNumber || u.PhoneNumber.Value == command.userNameEmailOrPhoneNumber,
            false,
            cancellationToken,
            u => u.UserOTPs, u => u.UserCredentials
            );
            if (user is null)
            {
                _logger.LogError("Can NOT find a user with these username/email/phone");
                return ErrorOr.Error.Failure("Can NOT find a user with these username/email/phone");
            }
            var activeOTP = user.UserOTPs.FirstOrDefault(otp => !otp.IsUsed && otp.ExpireAt > DateTime.UtcNow);
            if (activeOTP is null)
            {
                _logger.LogError("You do NOT has an otp. Please request one");
                return ErrorOr.Error.Failure("You do NOT has an otp. Please request one");
            }
            if (activeOTP.HashedOTP != command.otpToBeVerified)
            {
                _logger.LogError("OTP you entered is not valid");
                return ErrorOr.Error.Failure("OTP you entered is not valid");
            }
            activeOTP.MarkAsUsed();
            var accessToken = _jwtTokenGenerator.GenerateToken(user);
            var newCredentials = UserCredentials.Create(user.Id, command.newPassword);
            //user.AddUserCredential(newCredentials, _currentUser.?.ToString() ?? "unkown");
            //await _credentialsWriteRepo.AddAsync(newCredentials);
            _writeUserRepositry.Update(user);
            await _writeUnitOfWork.SaveChangesAsync();
            return new AuthResult(accessToken, "", DateTime.UtcNow.AddHours(1));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.InnerException);
            return Error.Failure(ex.Message);
        }
        
    }
}
