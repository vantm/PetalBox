﻿using ProductApi.Products.Domain;

namespace ProductApi.Products.Endpoints;

public class CreateProductRequest
{
    public string Title { get; init; } = string.Empty;
    public decimal Price { get; init; }
}

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(6).MaximumLength(200);
        RuleFor(x => x.Price).LessThan(1_000_000).GreaterThan(-1_000_000);
    }
}

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
        from mapper in rt.RequiredService<Mapper>()
        from dto in mapper.MapToProductDtoEff(product)
        select Results.CreatedAtRoute("GetProduct", new { dto.Id }, dto);
}
