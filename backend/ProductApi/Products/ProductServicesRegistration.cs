using ProductApi.Products.Domain;
using ProductApi.Products.Repo;
using ProductApi.Products.Services;
using ProductApi.Products.Services.Internals;

namespace ProductApi.Products;

public class ProductServicesRegistration : IServicesRegistration
{
    public void Add(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton(typeof(IProductRepo<>), typeof(DaprProductRepo<>));
        services.AddSingleton(typeof(IProductQueryService<>), typeof(DaprProductQueryService<>));
        services.AddSingleton<ProductMapper>();
        services.AddSingleton<ProductMapper>();
    }
}
