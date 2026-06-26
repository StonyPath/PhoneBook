using MediatR;
using PhoneBook.Application.DTOs;

namespace PhoneBook.Application.Commands.AddContact;

public record AddContactCommand(string FirstName, string LastName, string PhoneNumber, string Tag)
    : IRequest<ContactDto>;
