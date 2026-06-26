using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Repositories;
using PhoneBook.Domain.ValueObjects;
using System.Collections.Concurrent;

namespace PhoneBook.Infrastructure.Persistence;

public class InMemoryPhoneBookRepository : IPhoneBookRepository
{
    private readonly ConcurrentDictionary<Guid, PhoneBookRecord> _store = new();

    public Task<PhoneBookRecord?> GetByIdAsync(Guid id)
    {
        _store.TryGetValue(id, out var record);
        return Task.FromResult(record);
    }

    public Task<IEnumerable<PhoneBookRecord>> GetByTagAsync(Tag tag)
    {
        var results = _store.Values
            .Where(r => r.Tag.Equals(tag))
            .ToList();
        return Task.FromResult(results.AsEnumerable());
    }

    public Task AddAsync(PhoneBookRecord record)
    {
        if (!_store.TryAdd(record.Id, record))
            throw new InvalidOperationException("A record with the same ID already exists.");
        return Task.CompletedTask;
    }

    public Task UpdateAsync(PhoneBookRecord record)
    {
        _store[record.Id] = record;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _store.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByPhoneNumberAsync(PhoneNumber phoneNumber)
    {
        var exists = _store.Values.Any(r => r.PhoneNumber.Equals(phoneNumber));
        return Task.FromResult(exists);
    }
}
