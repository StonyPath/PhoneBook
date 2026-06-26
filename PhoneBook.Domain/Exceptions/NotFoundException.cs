namespace PhoneBook.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public override int HttpStatusCode => 404;
    public NotFoundException(string message) : base(message) { }
}
