using ProductApi.Products.Domain;

namespace ProductApi.Products.Endpoints;

public record GetProductEndpoint(
    Guid Id,
    IProductRepo Repo,
    CancellationToken CancelToken)
{
    public Task<IResult> Handle()
        => EitherWithError(Id)
        .LeftWhen(isDefault, () => Error.New("NotFound"))
        .BindAsync(x => Repo.FindAsync(x, CancelToken))
        .Match(Results.Ok, e =>
        {
            if (e.Message.Contains("NotFound"))
            {
                return Results.NotFound();
            }
            return Results.BadRequest(e.Message);
        });
}

