using ErrorOr;
using MediatR;
using RSAM.Application.Users.Common;
using RSAM.Domain.ValueObjects;

namespace RSAM.Application.Users.Commands;

public record UpdateUserPhoneNumberCommand
(
    Guid UserId,
    string PhoneNumber
):IRequest<ErrorOr<UserDto>>;
