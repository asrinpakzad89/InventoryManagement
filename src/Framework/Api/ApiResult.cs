namespace Framework.Api;

public class ApiResult
{
    public bool IsSuccess { get; set; }
    public List<string> Message { get; set; } = new();

    public ApiResult(bool isSuccess, List<string> message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    #region Implicit Operators
    public static implicit operator ApiResult(OkResult result)
    {
        return new ApiResult(true, new List<string>());
    }

    public static implicit operator ApiResult(BadRequestResult result)
    {
        return new ApiResult(false, new List<string>());
    }

    public static implicit operator ApiResult(BadRequestObjectResult result)
    {
        string message = result.Value == null ? "" : result.Value.ToString()!;
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct().Select(x => x).ToList();
            return new ApiResult(false, errorMessages);
        }
        return new ApiResult(false, new List<string> { message });
    }

    public static implicit operator ApiResult(ContentResult result)
    {
        string message = result.Content == null ? "" : result.Content.ToString()!;
        return new ApiResult(true, new List<string> { message });
    }

    public static implicit operator ApiResult(NotFoundResult result)
    {
        return new ApiResult(false, new List<string>());
    }

    public static implicit operator ApiResult(UnauthorizedResult result)
    {
        return new ApiResult(false, new List<string>());
    }
    #endregion
}

public class ApiResult<TData> : ApiResult
{
    public TData Data { get; set; }

    public ApiResult(bool isSuccess, TData data, List<string> message)
        : base(isSuccess, message)
    {
        Data = data;
    }

    public ApiResult(bool isSuccess, List<string> message)
        : base(isSuccess, message)
    {
    }

    #region Implicit Operators
    public static implicit operator ApiResult<TData>(TData data)
    {
        return new ApiResult<TData>(true, data, new List<string>());
    }

    public static implicit operator ApiResult<TData>(OkObjectResult result)
    {
        return new ApiResult<TData>(true, (TData)result.Value, new List<string>());
    }

    public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
    {
        string message = result.Value == null ? "" : result.Value.ToString()!;
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct().Select(x => x).ToList();
            return new ApiResult<TData>(false, errorMessages);
        }
        return new ApiResult<TData>(false, new List<string> { message });
    }

    public static implicit operator ApiResult<TData>(ContentResult result)
    {
        return new ApiResult<TData>(true, new List<string> { new string(result.Content!) });
    }

    public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
    {
        return new ApiResult<TData>(false, (TData)result.Value, new List<string> { new string("") });
    }
    #endregion
}
