using ProductApi.Products.Domain;

namespace ProductApi.Products.Endpoints.CreateProduct;

public readonly struct CreateProductEndpoint<RT>
    where RT : struct, HasCancel<RT>, HasServiceProvider
{
    public static Aff<RT, IResult> New(CreateProductRequest body) =>
        from rt in runtime<RT>()
        from __0 in guard(notnull(body), () => AppErrors.ValidationError("Body", "The request body must not be null"))
        from validator in rt.RequiredService<IValidator<CreateProductRequest>>()
        from validationResult in SuccessEff(validator.Validate(body))
        from __1 in guard(validationResult.IsValid, AppErrors.ValidationError(validationResult.ToDictionary()))
        from time in rt.RequiredService<TimeProvider>()
        from newProduct in SuccessEff(Product.New(new(body.Title), new(body.Price), time))
        from repo in rt.RequiredService<IProductRepo<RT>>()
        from product in repo.insert(newProduct)
        from mapper in rt.RequiredService<ProductMapper>()
        from dto in mapper.MapToProductDtoEff(product)
        select Results.CreatedAtRoute("GetProduct", new { dto.Id }, dto);
}

