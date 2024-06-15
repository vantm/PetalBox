namespace Common;

public static class ErrorHelper
{
    public static IResult MapError(Error error) =>
        error.Code switch
        {
            AppErrors.NotFoundCode => Results.NotFound(),
            AppErrors.ValidationErrorCode => MapValidationError(error),
            _ => Results.BadRequest(error.Message)
        };

    private static IResult MapValidationError(Error error) =>
        error is ValidationError validationError
            ? Results.ValidationProblem((validationError).Errors)
            : Results.BadRequest(error.Message);
}