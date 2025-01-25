namespace Common;

public record ValidationError(IDictionary<string, string[]> Errors) : Error()
{
    public override int Code => AppErrors.ValidationErrorCode;
    public override string Message => "The provided data is invalid.";
    public override bool IsExceptional => false;
    public override bool IsExpected => false;

    public override ErrorException ToErrorException() => ErrorException.New(Message);
}
