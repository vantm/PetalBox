namespace Common;

public static class ErrorCodes
{
    public const int NotFound = 5000;
    public const int ValidationProblem = 5100;

    public static readonly Error NotFoundError =
        Error.New(NotFound, "The data was not found");

    public static readonly Error ValidationProblemError(IEnumerable<string, string[]> errors) =>
        Error.New(ValidationProblem, "The provided data is invalid", )

}
