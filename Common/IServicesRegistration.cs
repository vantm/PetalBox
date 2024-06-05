namespace Common;

public interface IServicesRegistration
{
    void Add(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment);
}
