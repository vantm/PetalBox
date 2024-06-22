
namespace CustomerApi.Addresses;

public class AddressServicesRegistration : IServicesRegistration
{
    public void Add(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton<AddressMapper>();
    }
}
