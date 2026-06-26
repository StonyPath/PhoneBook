namespace PhoneBook.Domain.ValueObjects;

public class PhoneNumber : IEquatable<PhoneNumber>
{
    public string Number { get; }

    public PhoneNumber(string rawNumber)
    {
        if (string.IsNullOrWhiteSpace(rawNumber))
            throw new ArgumentException("Phone number cannot be empty.", nameof(rawNumber));

        var digits = new string(rawNumber.Where(char.IsDigit).ToArray());
        if (digits.Length < 7 || digits.Length > 15)
            throw new ArgumentException("Phone number must contain between 7 and 15 digits.", nameof(rawNumber));

        Number = digits;
    }

    public override bool Equals(object? obj) => Equals(obj as PhoneNumber);
    public bool Equals(PhoneNumber? other) => other is not null && Number == other.Number;
    public override int GetHashCode() => Number.GetHashCode();
    public override string ToString() => Number;
}
