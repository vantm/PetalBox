using CustomerApi.Addresses.Domain;
using CustomerApi.Addresses.Implementations;

namespace CustomerApi.Addresses;

public class AddressServicesRegistration : IServicesRegistration
{
    public void Add(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton<AddressMapper>();
        services.AddScoped<IAddressRepo, DaprAddressRepo>();
    }
}
