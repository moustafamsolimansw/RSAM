namespace RSAM.Contracts.Auth.Register;

public record DesingerOrClientRegisterRequest
(
    string FirstNameInEnglish,
    string? MiddleNameInEnglish,
    string LastNameInEnglish,
    string FirstNameInArabic,
    string? MiddleNameInArabic,
    string LastNameInArabic,
    string Organization,
    string Email,
    string PhoneNumber,
    string ProjectApplyingFor,
    bool HaveYouCompletedRoadSafetyAuditingForClientsCourse,
    string Password
);
