using FluentAssertions;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Domain.Test;

public class TagTests
{
    [Theory]
    [InlineData("Home")]
    [InlineData("my-colleague")]
    [InlineData("VIP")]
    [InlineData("Family123")]
    public void Create_ValidTag_ShouldStoreLowerCased(string input)
    {
        var tag = new Tag(input);
        tag.Value.Should().Be(input.ToLowerInvariant());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_Empty_ShouldThrow(string invalid)
    {
        Action act = () => new Tag(invalid);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("a")]           // too short
    [InlineData("thisTagIsWayTooLongForTheLimit")] // >30
    public void Create_InvalidLength_ShouldThrow(string tag)
    {
        Action act = () => new Tag(tag);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("home!")]       // special char
    [InlineData("work tag")]    // space
    public void Create_InvalidCharacters_ShouldThrow(string tag)
    {
        Action act = () => new Tag(tag);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equality_ShouldBeCaseInsensitive()
    {
        var tag1 = new Tag("HOME");
        var tag2 = new Tag("home");
        tag1.Should().Be(tag2);
    }
}
