using ErrorOr;
using MediatR;
using RSAM.Application.Users.Common;
using RSAM.Domain.Enums;

namespace RSAM.Application.Users.Commands;

public record UpdatePersonalInfoCommand
(
    Guid UserId,
    string FirstNameInEnglish,
    string? MiddleNameInEnglish,
    string LastNameInEnglish,
    string FirstNameInArabic,
    string? MiddleNameInArabic,
    string LastNameInArabic,
    DateOnly DateOfBirth,
    string Gender,
    AddressDto Address
) : IRequest<ErrorOr<UserDto>>;
public record AddressDto(string Street, string City, string State, string Country);