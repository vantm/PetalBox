namespace ProductApi.Products.Endpoints.AdjustProduct;

public class AdjustProductValidator : AbstractValidator<AdjustProductBody>
{
    public AdjustProductValidator()
    {
        RuleFor(x => x.Delta).NotEmpty();
    }
}
