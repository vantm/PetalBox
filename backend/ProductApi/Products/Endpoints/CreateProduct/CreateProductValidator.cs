namespace ProductApi.Products.Endpoints.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(6).MaximumLength(200);
        RuleFor(x => x.Price).LessThan(1_000_000).GreaterThan(-1_000_000);
    }
}

