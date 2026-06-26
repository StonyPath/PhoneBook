using FluentValidation;

namespace PhoneBook.Application.Commands.UpdateContact;

public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
{
    public UpdateContactCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[0-9\s\-\(\)]+$");
        RuleFor(x => x.Tag).NotEmpty().MaximumLength(50);
    }
}
