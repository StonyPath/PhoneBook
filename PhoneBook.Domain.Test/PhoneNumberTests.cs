using FluentAssertions;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Domain.Test;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("+1 (234) 567-8901", "12345678901")]
    [InlineData("123-456-7890", "1234567890")]
    [InlineData("1234567890", "1234567890")]
    [InlineData("   +44 123 456 789  ", "44123456789")]
    public void Create_ValidNumber_ShouldNormalizeToDigits(string input, string expectedDigits)
    {
        var phone = new PhoneNumber(input);
        phone.Number.Should().Be(expectedDigits);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_Empty_ShouldThrow(string invalid)
    {
        Action act = () => new PhoneNumber(invalid);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("12345")]     // too short
    [InlineData("1234567890123456")] // too long (16 digits)
    public void Create_InvalidLength_ShouldThrow(string number)
    {
        Action act = () => new PhoneNumber(number);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equality_ShouldBeBasedOnNormalizedDigits()
    {
        var phone1 = new PhoneNumber("+1-234-567-8901");
        var phone2 = new PhoneNumber("12345678901");
        phone1.Should().Be(phone2);
        phone1.GetHashCode().Should().Be(phone2.GetHashCode());
    }
}
