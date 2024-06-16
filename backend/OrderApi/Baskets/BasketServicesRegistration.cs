using OrderApi.Baskets.Services;
using OrderApi.Baskets.Services.Implementation;

namespace OrderApi.Baskets;

public class BasketServicesRegistration : IServicesRegistration
{
    public void Add(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton<DaprBasketHelper>();
        services.AddScoped<IBasketService, DaprBasketService>();
    }
}

