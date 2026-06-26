using MediatR;
using PhoneBook.Application.DTOs;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.Repositories;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Application.Commands.UpdateContact;

public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, ContactDto>
{
    private readonly IPhoneBookRepository _repository;

    public UpdateContactCommandHandler(IPhoneBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var record = await _repository.GetByIdAsync(request.Id);
        if (record is null)
            throw new NotFoundException($"Contact with ID {request.Id} not found.");


        if (record.PhoneNumber.Equals(request.PhoneNumber) == false)
        {
            var requestedPhoneNumber = new PhoneNumber(request.PhoneNumber);

            if (await _repository.ExistsByPhoneNumberAsync(requestedPhoneNumber))
                throw new ConflictException($"A contact with phone number '{request.PhoneNumber}' already exists.");
        }

        record.Update(
            request.FirstName,
            request.LastName,
            new PhoneNumber(request.PhoneNumber),
            new Tag(request.Tag));

        await _repository.UpdateAsync(record);

        return new ContactDto
        {
            Id = record.Id,
            FirstName = record.FirstName,
            LastName = record.LastName,
            PhoneNumber = record.PhoneNumber.Number,
            Tag = record.Tag.Value
        };
    }
}
