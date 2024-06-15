namespace OrderApi.Baskets;

public class BasketServicesRegistration : IServicesRegistration
{
    public void Add(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton<Mapper>();
    }
}

