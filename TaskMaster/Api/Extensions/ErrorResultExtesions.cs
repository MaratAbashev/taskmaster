using Domain.Utils;

namespace Api.Extensions;

public static class ErrorResultExtensions
{
    public static IResult ToErrorHttpResult<T>(this Result<T> result)
    {
        return result.ErrorType switch
        {
            ErrorType.NotFound => Results.NotFound(result.ErrorMessage),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Validation => Results.BadRequest(result.ErrorMessage),
            ErrorType.Conflict => Results.Conflict(result.ErrorMessage),
            ErrorType.Forbidden => Results.StatusCode(StatusCodes.Status403Forbidden),
            ErrorType.BadRequest => Results.BadRequest(result.ErrorMessage),
            _ => Results.Problem(result.ErrorMessage)
        };
    }

    public static IResult ToErrorHttpResult(this Result result)
    {
        return result.ErrorType switch
        {
            ErrorType.NotFound => Results.NotFound(result.ErrorMessage),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Validation => Results.BadRequest(result.ErrorMessage),
            ErrorType.Conflict => Results.Conflict(result.ErrorMessage),
            ErrorType.Forbidden => Results.StatusCode(StatusCodes.Status403Forbidden),
            ErrorType.BadRequest => Results.BadRequest(result.ErrorMessage),
            _ => Results.Problem(result.ErrorMessage)
        };
    }
}