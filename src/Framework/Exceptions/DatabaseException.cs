namespace Framework.Exceptions;

public class DatabaseException : Exception
{
    public HttpStatusCode HttpStatusCode { get; set; }

    public DatabaseException(HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
    {
        HttpStatusCode = httpStatusCode;
    }

    public DatabaseException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError) : base(message)
    {
        HttpStatusCode = httpStatusCode;
    }

    public DatabaseException(string message, Exception innerException, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(message, innerException)
    {
        HttpStatusCode = httpStatusCode;
    }

    protected DatabaseException(SerializationInfo info, StreamingContext context, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(info, context)
    {
        HttpStatusCode = httpStatusCode;
    }
}