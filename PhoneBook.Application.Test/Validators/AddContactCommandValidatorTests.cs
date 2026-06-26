using FluentAssertions;
using PhoneBook.Application.Commands.AddContact;

namespace PhoneBook.Application.Test.Validators;

public class AddContactCommandValidatorTests
{
    private readonly AddContactCommandValidator _validator = new();

    [Fact]
    public void Validate_AllFieldsValid_ShouldPass()
    {
        var command = new AddContactCommand("John", "Doe", "+1234567890", "work");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Doe", "+123", "tag", "FirstName")]
    [InlineData("John", "", "+123", "tag", "LastName")]
    [InlineData("John", "Doe", "", "tag", "PhoneNumber")]
    [InlineData("John", "Doe", "abc", "tag", "PhoneNumber")]
    [InlineData("John", "Doe", "+123", "", "Tag")]
    public void Validate_InvalidField_ShouldFail(string firstName, string lastName,
        string phone, string tag, string expectedProperty)
    {
        var command = new AddContactCommand(firstName, lastName, phone, tag);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == expectedProperty);
    }
}
