using FluentAssertions;
using Moq;
using PhoneBook.Application.Commands.DeleteContact;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.Repositories;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Application.Test.Commands;

public class DeleteContactCommandHandlerTests
{
    private readonly Mock<IPhoneBookRepository> _repoMock = new();
    private readonly DeleteContactCommandHandler _handler;

    public DeleteContactCommandHandlerTests()
    {
        _handler = new DeleteContactCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingId_ShouldDelete()
    {
        var record = new PhoneBookRecord("Test", "User", new PhoneNumber("123"), new Tag("test"));
        _repoMock.Setup(r => r.GetByIdAsync(record.Id)).ReturnsAsync(record);

        await _handler.Handle(new DeleteContactCommand(record.Id), CancellationToken.None);
        _repoMock.Verify(r => r.DeleteAsync(record.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentId_ShouldThrowNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((PhoneBookRecord?)null);

        Func<Task> act = () => _handler.Handle(new DeleteContactCommand(Guid.NewGuid()), CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
