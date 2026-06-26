using FluentAssertions;
using Moq;
using PhoneBook.Application.Commands.AddContact;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.Repositories;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Application.Test.Commands;

public class AddContactCommandHandlerTests
{
    private readonly Mock<IPhoneBookRepository> _repoMock = new();
    private readonly AddContactCommandHandler _handler;

    public AddContactCommandHandlerTests()
    {
        _handler = new AddContactCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_UniqueNumber_ShouldAddAndReturnDto()
    {
        _repoMock.Setup(r => r.ExistsByPhoneNumberAsync(It.IsAny<PhoneNumber>()))
                 .ReturnsAsync(false);
        var command = new AddContactCommand("Jane", "Doe", "+999", "friend");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Doe");
        result.PhoneNumber.Should().Be("+999");
        result.Tag.Should().Be("friend");
        result.Id.Should().NotBeEmpty();
        _repoMock.Verify(r => r.AddAsync(It.IsAny<PhoneBookRecord>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateNumber_ShouldThrowConflict()
    {
        _repoMock.Setup(r => r.ExistsByPhoneNumberAsync(It.IsAny<PhoneNumber>()))
                 .ReturnsAsync(true);
        var command = new AddContactCommand("Jane", "Doe", "+999", "friend");

        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<ConflictException>();
        _repoMock.Verify(r => r.AddAsync(It.IsAny<PhoneBookRecord>()), Times.Never);
    }
}
