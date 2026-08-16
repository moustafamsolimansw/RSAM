using ErrorOr;
using MediatR;
using RSAM.Application.Users.Common;

namespace RSAM.Application.Users.Commands;

public record CreateUserCommand
(
    string username,
    string email,
    string firstNameInEnglish,
    string? middleNameInEnglish,
    string lastNameInEnglish,
    string firstNameInArabic,
    string? middleNameInArabic,
    string lastNameInArabic,
    DateOnly dateOfBirth,
    string gender,
    string street,
    string city,
    string state,
    string country,
    string phoneNumber
) : IRequest<ErrorOr<UserDto>>;
