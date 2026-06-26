using MediatR;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.Repositories;

namespace PhoneBook.Application.Commands.DeleteContact;

public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand>
{
    private readonly IPhoneBookRepository _repository;

    public DeleteContactCommandHandler(IPhoneBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var record = await _repository.GetByIdAsync(request.Id);
        if (record is null)
            throw new NotFoundException($"Contact with ID {request.Id} not found.");

        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
