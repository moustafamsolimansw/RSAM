using System.Reflection;

namespace RSAM.Contracts.Users.Commands;

public record UpdateUserPersonalInfoRequest
(
    string FirstNameInEnglish,
    string? MiddleNameInEnglish,
    string LastNameInEnglish,
    string FirstNameInArabic,
    string? MiddleNameInArabic,
    string LastNameInArabic,
    DateOnly DateOfBirth,
    string Gender,
    string Street,
    string City,
    string State,
    string Country
);
