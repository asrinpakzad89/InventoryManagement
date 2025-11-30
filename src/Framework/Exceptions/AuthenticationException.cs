namespace Framework.Exceptions;

public class AuthenticationException : Exception
{
    public HttpStatusCode HttpStatusCode { get; set; }

    public AuthenticationException(HttpStatusCode httpStatusCode = HttpStatusCode.Unauthorized)
    {
        HttpStatusCode = httpStatusCode;
    }

    public AuthenticationException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.Unauthorized) : base(message)
    {
        HttpStatusCode = httpStatusCode;
    }

    public AuthenticationException(string message, Exception innerException, HttpStatusCode httpStatusCode = HttpStatusCode.Unauthorized) : base(message, innerException)
    {
        HttpStatusCode = httpStatusCode;
    }

    protected AuthenticationException(SerializationInfo info, StreamingContext context, HttpStatusCode httpStatusCode = HttpStatusCode.Unauthorized) : base(info, context)
    {
        HttpStatusCode = httpStatusCode;
    }
}