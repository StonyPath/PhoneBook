using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Domain.Entities;

public class PhoneBookRecord
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public Tag Tag { get; private set; }

    internal PhoneBookRecord() { }

    public PhoneBookRecord(string firstName, string lastName, PhoneNumber phoneNumber, Tag tag)
    {
        Id = Guid.NewGuid();
        SetFirstName(firstName);
        SetLastName(lastName);
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        Tag = tag ?? throw new ArgumentNullException(nameof(tag));
    }

    public void Update(string firstName, string lastName, PhoneNumber phoneNumber, Tag tag)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        Tag = tag ?? throw new ArgumentNullException(nameof(tag));
    }

    private void SetFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        FirstName = firstName.Trim();
    }

    private void SetLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        LastName = lastName.Trim();
    }
}
