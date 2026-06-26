using FluentAssertions;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.ValueObjects;
using PhoneBook.Infrastructure.Persistence;

namespace PhoneBook.Infrastructure.Test.Persistence;

public class InMemoryPhoneBookRepositoryTests
{
    private readonly InMemoryPhoneBookRepository _repo = new();

    [Fact]
    public async Task AddAndGetById_ShouldReturnRecord()
    {
        var record = new PhoneBookRecord("John", "Doe", new PhoneNumber("123"), new Tag("home"));
        await _repo.AddAsync(record);

        var retrieved = await _repo.GetByIdAsync(record.Id);
        retrieved.Should().BeEquivalentTo(record);
    }

    [Fact]
    public async Task GetByTag_ShouldFilterCorrectly()
    {
        var tag1 = new Tag("work");
        var tag2 = new Tag("home");
        await _repo.AddAsync(new PhoneBookRecord("A", "B", new PhoneNumber("1"), tag1));
        await _repo.AddAsync(new PhoneBookRecord("C", "D", new PhoneNumber("2"), tag2));
        await _repo.AddAsync(new PhoneBookRecord("E", "F", new PhoneNumber("3"), tag1));

        var result = await _repo.GetByTagAsync(tag1);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Update_ShouldModifyExisting()
    {
        var record = new PhoneBookRecord("Old", "Name", new PhoneNumber("111"), new Tag("old"));
        await _repo.AddAsync(record);
        record.Update("New", "Name", new PhoneNumber("222"), new Tag("new"));
        await _repo.UpdateAsync(record);

        var updated = await _repo.GetByIdAsync(record.Id);
        updated!.FirstName.Should().Be("New");
        updated.PhoneNumber.Number.Should().Be("222");
    }

    [Fact]
    public async Task Delete_ShouldRemoveRecord()
    {
        var record = new PhoneBookRecord("Del", "Test", new PhoneNumber("000"), new Tag("temp"));
        await _repo.AddAsync(record);
        await _repo.DeleteAsync(record.Id);

        var result = await _repo.GetByIdAsync(record.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsByPhoneNumber_ShouldDetectDuplicates()
    {
        var phone = new PhoneNumber("+123");
        await _repo.AddAsync(new PhoneBookRecord("A", "B", phone, new Tag("x")));

        var exists = await _repo.ExistsByPhoneNumberAsync(phone);
        exists.Should().BeTrue();

        var notExists = await _repo.ExistsByPhoneNumberAsync(new PhoneNumber("+999"));
        notExists.Should().BeFalse();
    }
}