using ValidationException = FluentValidation.ValidationException;

namespace Framework.Extensions;

public static class HttpResponseExtensions
{
    public static Task WriteValidationErrorsAsync(this HttpResponse response, ValidationException ex)
    {
        var errors = ex.Errors.Select(cur => cur.ErrorMessage)
            .Distinct()
            .Select(x => x)
            .ToList();

        if (errors != null && errors.Any())
        {
            return response.WriteResponseAsync(HttpStatusCode.OK, new ApiResult(false, errors));
        }
        return response.WriteResponseAsync(HttpStatusCode.OK, new ApiResult(false, new List<string> { ex.Message }));
    }

    public static Task WriteApplicationErrorsAsync(this HttpResponse response, AppException ex)
    {
        return response.WriteResponseAsync(ex.HttpStatusCode, new ApiResult(false, new List<string> { ex.Message }));
    }

    public static Task WriteBusinessErrorsAsync(this HttpResponse response, BusinessException ex)
    {
        return response.WriteResponseAsync(HttpStatusCode.BadRequest, new ApiResult(false, new List<string> { ex.Message }));
    }

    public static Task WriteNotFoundErrorsAsync(this HttpResponse response, NotFoundException ex)
    {
        return response.WriteResponseAsync(HttpStatusCode.OK, new ApiResult(false, new List<string> { ex.Message }));
    }

    public static Task WriteDuplicateErrorsAsync(this HttpResponse response, DuplicateException ex)
    {
        return response.WriteResponseAsync(HttpStatusCode.OK, new ApiResult(false, new List<string> { ex.Message }));
    }

    public static Task WriteDatabaseErrorsAsync(this HttpResponse response, DatabaseException ex)
    {
        return response.WriteResponseAsync(ex.HttpStatusCode, new ApiResult(false, new List<string> { ex.Message }));
    }

    public static Task WriteAuthenticationErrorsAsync(this HttpResponse response, AuthenticationException ex)
    {
        return response.WriteResponseAsync(ex.HttpStatusCode, new ApiResult(false, new List<string> { ex.Message }));
    }

    public static Task WriteUnhandledExceptionsAsync(this HttpResponse response)
    {
        return response.WriteResponseAsync(HttpStatusCode.BadRequest, new ApiResult(false, new List<string> { "Internal server error" }));
    }

    private static Task WriteResponseAsync(this HttpResponse response, HttpStatusCode statusCodes, ApiResult apiResult)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
        };

        response.StatusCode = (int)statusCodes;
        response.ContentType = "application/json";

        return response.WriteAsync(JsonConvert.SerializeObject(apiResult, settings));
    }
}

