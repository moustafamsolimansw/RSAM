using RSAM.Domain.Enums;
using RSAM.Domain.SharedModels;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Domain.ValueObjects;

public class PersonInfo : ValueObject
{
    public string FirstNameInEnglish { get; private set; }
    public string? MiddleNameInEnglish { get; private set; }
    public string LastNameInEnglish { get; private set; }
    public string FirstNameInArabic { get; private set; }
    public string? MiddleNameInArabic { get; private set; }
    public string LastNameInArabic { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public Address Address { get; private set; }

#pragma warning disable CS8618
    private PersonInfo() { }
#pragma warning restore CS8618
    private PersonInfo(string firstNameInEnglish, string? middleNameInEnglish, string lastNameInEnglish,
        string firstNameInArabic, string? middleNameInArabic, string lastNameInArabic, DateOnly dateOfBirth, Gender gender, Address address)
    {
        FirstNameInEnglish = firstNameInEnglish;
        MiddleNameInEnglish = middleNameInEnglish;
        LastNameInEnglish = lastNameInEnglish;
        FirstNameInArabic = firstNameInArabic;
        MiddleNameInArabic = middleNameInArabic;
        LastNameInArabic = lastNameInArabic;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        Address = address;
    }
    public static PersonInfo Create(string firstNameInEnglish, string? middleNameInEnglish, string lastNameInEnglish,
        string firstNameInArabic, string? middleNameInArabic, string lastNameInArabic, DateOnly dateOfBirth, Gender gender, Address address)
    {
        return new PersonInfo(firstNameInEnglish, middleNameInEnglish, lastNameInEnglish,
            firstNameInArabic, middleNameInArabic, lastNameInArabic, dateOfBirth, gender, address);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstNameInEnglish;
        yield return MiddleNameInEnglish ?? "";
        yield return LastNameInEnglish;
        yield return FirstNameInArabic;
        yield return MiddleNameInArabic ?? "";
        yield return LastNameInArabic;
        yield return DateOfBirth;
        yield return Gender;
        yield return Address;
    }
}
