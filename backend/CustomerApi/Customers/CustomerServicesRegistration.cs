
using CustomerApi.Addresses;

namespace CustomerApi.Customers;

public class CustomerServicesRegistration : IServicesRegistration
{
    public void Add(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton<AddressMapper>();
    }
}
