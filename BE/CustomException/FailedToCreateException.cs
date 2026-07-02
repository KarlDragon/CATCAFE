namespace BE.Exceptions;

public class FailedToCreateException : Exception
{
    public FailedToCreateException(string message) : base(message) { }
}