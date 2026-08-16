using ErrorOr;
using MediatR;
using RSAM.Application.Users.Common;

namespace RSAM.Application.Users.Commands;

public record ResetPasswordCommand
(string userNameEmailOrPhoneNumber, string otpToBeVerified, string newPassword, string confirmNewPassword) : IRequest<ErrorOr<AuthResult>>;
