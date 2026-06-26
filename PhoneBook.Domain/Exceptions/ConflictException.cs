namespace PhoneBook.Domain.Exceptions;

public class ConflictException : DomainException
{
    public override int HttpStatusCode => 409;
    public ConflictException(string message) : base(message) { }
}
