using Common.Implements;

namespace Common;

public class CommonServicesRegistration : IServicesRegistration
{
    public void Add(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddValidatorsFromAssemblyContaining<CommonServicesRegistration>();

        services.AddScoped<IServiceBus, DaprServiceBus>();

        services
            .AddOptionsWithValidateOnStart<CommonOptions>()
            .BindConfiguration(CommonOptions.Name)
            .ValidateDataAnnotations();

        services.AddHttpContextAccessor();

        services.AddScoped<WebRuntime>();
    }
}
