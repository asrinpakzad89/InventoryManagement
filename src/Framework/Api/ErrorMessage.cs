namespace Framework.Api;

public class ErrorMessage
{
    public ErrorCode Code { get; set; } = ErrorCode.ServerError;
    public string Message { get; set; } = "";
    public ErrorMessage()
    {
    }

    public ErrorMessage(ErrorCode code, string message)
    {
        Code = code;
        Message = message;
    }
}
