using Microsoft.AspNetCore.Mvc;
using ProductApi.Products.Domain;

namespace ProductApi.Products.Endpoints;

public record ListProductEndpoint(
    [FromQuery(Name = "p")] int? Page,
    [FromQuery(Name = "s")] int? Size,
    IProductRepo Repo,
    IValidator<ListProductEndpoint> Validator,
    CancellationToken CancelToken)
{
    public Task<IResult> Handle()
    {
        var validationResult = Validator.Validate(this);
        return EitherWithError(SelectParams.FromPaging(Page, Size))
            .LeftWhen(_ => !validationResult.IsValid, () => Error.New("ValidationError"))
            .BindAsync(@params => Repo.SelectAsync(@params, CancelToken))
            .Match(Results.Ok, e =>
            {
                if (e.Message.Contains("ValidationError"))
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                return Results.BadRequest(e.Message);
            });
    }

    public class EndpointValidator : AbstractValidator<ListProductEndpoint>
    {
        public EndpointValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .LessThanOrEqualTo(2_000_000)
                .OverridePropertyName("p");

            RuleFor(x => x.Size)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(1000)
                .OverridePropertyName("s");
        }
    }
}

