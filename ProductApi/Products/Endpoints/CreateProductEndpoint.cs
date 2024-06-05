using ProductApi.Products.Domain;

namespace ProductApi.Products.Endpoints;

public class CreateProductRequest
{
    public string Title { get; init; } = string.Empty;
    public decimal Price { get; init; }
}

public record CreateProductEndpoint(
    CreateProductRequest Body,
    IValidator<CreateProductRequest> Validator,
    IProductRepo Repo,
    TimeProvider Time,
    CancellationToken CancelToken)
{
    public Task<IResult> Handle()
    {
        var validationResult = Validator.Validate(Body);
        return EitherWithError(Body)
            .LeftWhen(isDefault, () => Error.New("Body.Null"))
            .LeftWhen(!validationResult.IsValid, () => Error.New("ValidationError"))
            .Map(x => Product.New(x.Title, x.Price, Time))
            .MapAsync(async product =>
            {
                await Repo.InsertAsync(product, CancelToken);
                return product;
            })
            .Match(p => Results.CreatedAtRoute("GetProduct", new { p.Id }, p), (e) =>
            {
                if (e.Message.Contains("ValidationError"))
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                return Results.BadRequest(e.Message);
            });
    }

    public class EndpointValidator : AbstractValidator<CreateProductRequest>
    {
        public EndpointValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(6).MaximumLength(200);
            RuleFor(x => x.Price).LessThan(1_000_000).GreaterThan(-1_000_000);
        }
    }
}

