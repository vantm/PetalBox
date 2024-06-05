using System.Reflection;

namespace Common;

public static class ServiceCollectionExtensions
{
    public static void AddServicesRegistrations(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddServicesRegistrations(
            configuration, environment, Assembly.GetEntryAssembly()!);
    }

    public static void AddServicesRegistrations(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment,
        params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            PrivateAddServicesRegistrations(services, configuration, environment, assembly);
        }
    }

    private static void PrivateAddServicesRegistrations(
        IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment,
        Assembly assembly)
    {
        var matchedTypes = from type in assembly.GetTypes()
                           where type.IsClass && !type.IsAbstract && type.IsAssignableTo(typeof(IServicesRegistration))
                           select type;

        foreach (var matchedType in matchedTypes)
        {
            var instance = (IServicesRegistration)Activator.CreateInstance(matchedType)!;
            instance.Add(services, configuration, environment);
        }
    }
}
