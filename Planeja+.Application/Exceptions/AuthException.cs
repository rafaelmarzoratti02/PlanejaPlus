namespace Planeja_.Application.Exceptions;

public sealed class AuthException : Exception
{
    public AuthException(string message) : base(message) { }
}
