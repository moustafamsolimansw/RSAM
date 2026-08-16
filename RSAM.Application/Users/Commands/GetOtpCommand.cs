using ErrorOr;
using MediatR;
using RSAM.Application.Users.Common;

namespace RSAM.Application.Users.Commands;

public record GetOtpCommand(
    string usernameOrEmailOrPhoneNumber
    ) : IRequest<ErrorOr<bool>>;
