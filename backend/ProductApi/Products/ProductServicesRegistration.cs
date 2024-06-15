using ProductApi.Products.Domain;
using ProductApi.Products.Repo;

namespace ProductApi.Products;

public class ProductServicesRegistration : IServicesRegistration
{
    public void Add(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton(typeof(IProductRepo<>), typeof(DaprProductRepo<>));
        services.AddSingleton<DaprProductRepoHelper>();
        services.AddSingleton<Mapper>();
    }
}
