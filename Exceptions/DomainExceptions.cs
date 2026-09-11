namespace CinemaProj.Exceptions;

public class ValidationException(string code, string msg) : Exception(msg)
{
    public string Code { get; } = code;
}

public class DuplicateScreeningException(string msg) : Exception(msg);
