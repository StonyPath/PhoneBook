using FluentValidation;

namespace PhoneBook.Application.Commands.AddContact;

public class AddContactCommandValidator : AbstractValidator<AddContactCommand>
{
    public AddContactCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PhoneNumber).NotEmpty()
                                   .Matches(@"^\+?[0-9\s\-\(\)]+$")
                                   .WithMessage("Phone number must contain only digits, spaces, dashes, parentheses or leading '+'.");
        RuleFor(x => x.Tag).NotEmpty().MaximumLength(50);
    }
}
