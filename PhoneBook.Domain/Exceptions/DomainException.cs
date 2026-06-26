namespace PhoneBook.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public abstract int HttpStatusCode { get; }
    protected DomainException(string message) : base(message) { }
}