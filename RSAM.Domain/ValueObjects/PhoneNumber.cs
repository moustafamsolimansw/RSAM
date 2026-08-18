using RSAM.Domain.SharedModels;
using System.Text.RegularExpressions;

namespace RSAM.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    private static readonly Regex PhoneNumberRegex = new(
        @"^\+?[1-9]\d{7,14}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    );
    public string Value { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private PhoneNumber() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private PhoneNumber(string value)
    {
        Value = value;
    }
    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Phone number is required.",
                nameof(value));

        value = Normalize(value);
        /*
        if (!PhoneNumberRegex.IsMatch(value))
            throw new ArgumentException(
                $"Invalid phone number: {value}",
                nameof(value));
        */
        return new PhoneNumber(value);
    }
    private static string Normalize(string value)
    {
        return value
            .Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");
    }

    public bool Equals(PhoneNumber? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Value == other.Value;
    }

    public override bool Equals(object? obj)
        => obj is PhoneNumber other && Equals(other);

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(PhoneNumber phoneNumber)
        => phoneNumber.Value;
}
