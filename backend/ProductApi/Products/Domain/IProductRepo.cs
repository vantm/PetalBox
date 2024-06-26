﻿namespace ProductApi.Products.Domain;

public interface IProductRepo<RT>
    where RT : struct, HasCancel<RT>, HasServiceProvider
{
    Aff<RT, Product> of(ProductId id);
    Aff<RT, Seq<Product>> all(SelectParams @params);
    Aff<RT, Product> insert(Product product);
    Aff<RT, Product> update(Product product);
}
