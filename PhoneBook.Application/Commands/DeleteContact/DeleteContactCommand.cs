using MediatR;

namespace PhoneBook.Application.Commands.DeleteContact;

public record DeleteContactCommand(Guid Id) : IRequest;
