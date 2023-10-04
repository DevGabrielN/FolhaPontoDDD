namespace FolhaPontoDDD.Domain.Models.Exceptions;

public class DomainException : ApplicationException
{
    public DomainException(string message) : base(message) { }
}
