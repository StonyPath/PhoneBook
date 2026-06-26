using FluentAssertions;
using Moq;
using PhoneBook.Application.Commands.UpdateContact;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.Repositories;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Application.Test.Commands;

public class UpdateContactCommandHandlerTests
{
    private readonly Mock<IPhoneBookRepository> _repoMock = new();
    private readonly UpdateContactCommandHandler _handler;
    private readonly Guid _existingId = Guid.NewGuid();

    public UpdateContactCommandHandlerTests()
    {
        _handler = new UpdateContactCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingRecord_ShouldUpdateAndReturnDto()
    {
        var record = new PhoneBookRecord("Old", "Name", new PhoneNumber("111"), new Tag("old"));
        _repoMock.Setup(r => r.GetByIdAsync(_existingId)).ReturnsAsync(record);

        var command = new UpdateContactCommand(_existingId, "New", "Name", "222", "new");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.FirstName.Should().Be("New");
        result.PhoneNumber.Should().Be("222");
        result.Tag.Should().Be("new");
        _repoMock.Verify(r => r.UpdateAsync(record), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentId_ShouldThrowNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((PhoneBookRecord?)null);
        var command = new UpdateContactCommand(Guid.NewGuid(), "x", "y", "123", "tag");

        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
