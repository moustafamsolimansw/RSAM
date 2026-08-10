using RSAM.Domain.SharedModels;
using System.Text.RegularExpressions;

namespace RSAM.Domain.ValueObjects;

public class EmailAddress : ValueObject
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);
    public string Value { get; private set; }
#pragma warning disable CS8618
    private EmailAddress() { }
#pragma warning restore CS8618
    private EmailAddress(string value)
    {
        Value = value;
    }
    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email address cannot be null or empty.", nameof(value));
        }
        if (!EmailRegex.IsMatch(value))
        {
            throw new ArgumentException("Invalid email address format.", nameof(value));
        }
        return new EmailAddress(value);
    }
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    override public string ToString()
    {
        return Value;
    }
}
