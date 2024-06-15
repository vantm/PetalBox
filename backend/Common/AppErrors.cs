namespace Common;

public static class AppErrors
{
    public const int DbErrorCode = 1000;
    public const int ArgNull = 2000;
    public const int NotFoundCode = 100_000;
    public const int ValidationErrorCode = 110_000;

    public static readonly Error NotFoundError =
        Error.New(NotFoundCode, "The data was not found");

    public static Error DbError(string message) =>
        Error.New(DbErrorCode, message);

    public static Error ValidationError(IDictionary<string, string[]> errors) =>
        new ValidationError(errors);

    public static Error ValidationError(string memberName, string value) =>
        new ValidationError(new Dictionary<string, string[]> { [memberName] = [value] });
}
