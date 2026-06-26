using PhoneBook.Domain.Entities;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Domain.Repositories;

public interface IPhoneBookRepository
{
    Task<PhoneBookRecord?> GetByIdAsync(Guid id);
    Task<IEnumerable<PhoneBookRecord>> GetByTagAsync(Tag tag);
    Task AddAsync(PhoneBookRecord record);
    Task UpdateAsync(PhoneBookRecord record);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByPhoneNumberAsync(PhoneNumber phoneNumber);
}