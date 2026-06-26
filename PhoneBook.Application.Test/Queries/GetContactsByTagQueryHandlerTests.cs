using FluentAssertions;
using Moq;
using PhoneBook.Application.Queries.GetContactsByTag;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Repositories;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Application.Test.Queries;

public class GetContactsByTagQueryHandlerTests
{
    private readonly Mock<IPhoneBookRepository> _repoMock = new();
    private readonly GetContactsByTagQueryHandler _handler;

    public GetContactsByTagQueryHandlerTests()
    {
        _handler = new GetContactsByTagQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDtos()
    {
        var tag = new Tag("work");
        var records = new[]
        {
            new PhoneBookRecord("A", "B", new PhoneNumber("111"), tag),
            new PhoneBookRecord("C", "D", new PhoneNumber("222"), tag)
        };
        _repoMock.Setup(r => r.GetByTagAsync(tag)).ReturnsAsync(records);

        var result = await _handler.Handle(new GetContactsByTagQuery("work"), CancellationToken.None);

        result.Should().HaveCount(2);
        result.First().Tag.Should().Be("work");
    }
}
