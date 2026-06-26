using MediatR;
using PhoneBook.Application.DTOs;
using PhoneBook.Domain.Repositories;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Application.Queries.GetContactsByTag;

public class GetContactsByTagQueryHandler : IRequestHandler<GetContactsByTagQuery, IEnumerable<ContactDto>>
{
    private readonly IPhoneBookRepository _repository;

    public GetContactsByTagQueryHandler(IPhoneBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ContactDto>> Handle(GetContactsByTagQuery request, CancellationToken cancellationToken)
    {
        var tag = new Tag(request.Tag);
        var records = await _repository.GetByTagAsync(tag);

        return records.Select(r => new ContactDto
        {
            Id = r.Id,
            FirstName = r.FirstName,
            LastName = r.LastName,
            PhoneNumber = r.PhoneNumber.Number,
            Tag = r.Tag.Value
        });
    }
}
