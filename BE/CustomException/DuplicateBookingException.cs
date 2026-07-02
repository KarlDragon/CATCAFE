namespace BE.Exceptions;

public class DuplicateBookingException : Exception
{
    public DuplicateBookingException(string message) : base(message) { }
}