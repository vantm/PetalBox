using Dapr.Client;
using Microsoft.Extensions.Options;
using ProductApi.Products.Domain;

namespace ProductApi.Products.Repo;

public class DaprProductRepoHelper(IOptions<CommonOptions> commonOptions)
{
    public BindingRequest PrepareSelectStatement(SelectParams @params)
    {
        const string SqlText = """
            SELECT id, title, price, is_active, quantity
            FROM product
            OFFSET $1
            LIMIT $2
            """;

        return new BindingRequest(commonOptions.Value.BindingName, "query")
            .AddSqlText(SqlText)
            .AddSqlParams(@params.Offset, @params.Limit);
    }

    public BindingRequest PrepareFindStatement(ProductId id)
    {
        const string SqlText = """
            SELECT id, title, price, is_active, quantity
            FROM product
            WHERE id = $1
            """;

        return new BindingRequest(commonOptions.Value.BindingName, "query")
            .AddSqlText(SqlText)
            .AddSqlParams(id.Value);
    }

    public BindingRequest PrepareInsertStatement(Product product)
    {
        const string SqlText = """
            INSERT INTO product (id, title, price, is_active, quantity)
            VALUES ($1, $2, $3, $4, $5)
            """;

        return new BindingRequest(commonOptions.Value.BindingName, "exec")
            .AddSqlText(SqlText)
            .AddSqlParams(
                product.Id.Value,
                product.Title.Value,
                product.Price.Value,
                product.IsActive,
                product.Quantity.Value);
    }

    public BindingRequest PrepareUpdateStatement(Product product)
    {
        const string SqlText = """
            UPDATE product
            SET title = $2, price = $3, is_active= $4, quantity = $5
            WHERE id = $1
            """;

        return new BindingRequest(commonOptions.Value.BindingName, "exec")
            .AddSqlText(SqlText)
            .AddSqlParams(
                product.Id.Value,
                product.Title.Value,
                product.Price.Value,
                product.IsActive,
                product.Quantity.Value);
    }
}
