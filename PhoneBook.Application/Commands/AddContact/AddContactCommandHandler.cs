using MediatR;
using PhoneBook.Application.DTOs;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.Repositories;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Application.Commands.AddContact;

public class AddContactCommandHandler : IRequestHandler<AddContactCommand, ContactDto>
{
    private readonly IPhoneBookRepository _repository;

    public AddContactCommandHandler(IPhoneBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactDto> Handle(AddContactCommand request, CancellationToken cancellationToken)
    {
        var phoneNumber = new PhoneNumber(request.PhoneNumber);
        var tag = new Tag(request.Tag);

        if (await _repository.ExistsByPhoneNumberAsync(phoneNumber))
            throw new ConflictException($"A contact with phone number '{phoneNumber}' already exists.");

        var record = new PhoneBookRecord(request.FirstName, request.LastName, phoneNumber, tag);
        await _repository.AddAsync(record);

        return MapToDto(record);
    }

    private static ContactDto MapToDto(PhoneBookRecord record) => new()
    {
        Id = record.Id,
        FirstName = record.FirstName,
        LastName = record.LastName,
        PhoneNumber = record.PhoneNumber.Number,
        Tag = record.Tag.Value
    };
}
