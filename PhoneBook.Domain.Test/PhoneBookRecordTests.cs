using FluentAssertions;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Domain.Test;

public class PhoneBookRecordTests
{
    [Fact]
    public void Create_ValidData_ShouldAssignProperties()
    {
        var record = new PhoneBookRecord("John", "Doe",
            new PhoneNumber("+1234567890"), new Tag("work"));

        record.Id.Should().NotBeEmpty();
        record.FirstName.Should().Be("John");
        record.LastName.Should().Be("Doe");
        record.PhoneNumber.Number.Should().Be("+1234567890");
        record.Tag.Value.Should().Be("work");
    }

    [Theory]
    [InlineData("", "Doe")]
    [InlineData(" ", "Doe")]
    [InlineData(null, "Doe")]
    public void Create_InvalidFirstName_ShouldThrow(string firstName, string lastName)
    {
        Action act = () => new PhoneBookRecord(firstName, lastName,
            new PhoneNumber("123"), new Tag("tag"));
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_ShouldChangeAllValues()
    {
        var record = new PhoneBookRecord("Old", "Name",
            new PhoneNumber("111"), new Tag("old"));

        record.Update("New", "Name", new PhoneNumber("222"), new Tag("new"));

        record.FirstName.Should().Be("New");
        record.LastName.Should().Be("Name");
        record.PhoneNumber.Number.Should().Be("222");
        record.Tag.Value.Should().Be("new");
    }
}
