using MediatR;
using PhoneBook.Application.DTOs;

namespace PhoneBook.Application.Commands.UpdateContact;


public record UpdateContactCommand(Guid Id, string FirstName, string LastName, string PhoneNumber, string Tag)
    : IRequest<ContactDto>;
