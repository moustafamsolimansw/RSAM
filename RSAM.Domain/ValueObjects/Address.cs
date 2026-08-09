using RSAM.Domain.SharedModels;

namespace RSAM.Domain.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }

#pragma warning disable CS8618
    private Address() { }
#pragma warning restore CS8618
    private Address(string street, string city, string state, string country)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
    }
    public static Address Create(string street, string city, string state, string country)
    {
        return new Address(street, city, state, country);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
    }
}
