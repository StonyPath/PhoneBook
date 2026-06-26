using MediatR;
using PhoneBook.Application.DTOs;

namespace PhoneBook.Application.Queries.GetContactsByTag;

public record GetContactsByTagQuery(string Tag) : IRequest<IEnumerable<ContactDto>>;