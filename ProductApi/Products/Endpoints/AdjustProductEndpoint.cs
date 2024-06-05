using ProductApi.Products.Domain;

namespace ProductApi.Products.Endpoints;

public class AdjustProductRequest
{
    public int Delta { get; init; }
}

public record AdjustProductEndpoint(
    Guid Id,
    AdjustProductRequest Body,
    IValidator<AdjustProductRequest> Validator,
    IProductRepo Repo,
    CancellationToken CancelToken)
{
    public Task<IResult> Handle()
    {
        var validationResult = Validator.Validate(Body);
        return EitherWithError(Id)
            .LeftWhen(isDefault, () => Error.New("NotFound"))
            .LeftWhen(!validationResult.IsValid, () => Error.New("ValidationError"))
            .BindAsync(x => Repo.FindAsync(x, CancelToken))
            .LeftWhenAsync(isDefault, () => Error.New("NotFound"))
            .BindAsync(product =>
            {
                product.Adjust(Body.Delta);
                return Repo.UpdateAsync(product, CancelToken);
            })
            .Match(_ => Results.NoContent(), e =>
            {
                if (e.Message.Contains("NotFound"))
                {
                    return Results.NotFound();
                }
                if (e.Message.Contains("ValidationError"))
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                return Results.BadRequest(e.Message);
            });
    }

    public class EndpointValidator : AbstractValidator<AdjustProductRequest>
    {
        public EndpointValidator()
        {
            RuleFor(x => x.Delta).NotEmpty();
        }
    }
}
