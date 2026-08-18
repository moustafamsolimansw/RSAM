using ErrorOr;
using MediatR;

namespace RSAM.Application.Users.Common;

public record UserDto
(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string Username,
    string PhoneNumber
);
