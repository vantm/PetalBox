using Common.Dapr.Internals;
using Common.Domain;

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

        services.AddScoped<IAppRuntime>(sp =>
        {
            var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext == null)
            {
                throw new ValueIsNullException("The scoped context has no access to HttpContext");
            }
            return new AppRuntime(httpContext);
        });

        services.AddSingleton(TimeProvider.System);

        services.AddDaprClient();
    }
}
