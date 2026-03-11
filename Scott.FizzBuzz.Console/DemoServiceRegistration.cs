using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Scott.FizzBuzz.Core.Interfaces;

namespace Scott.FizzBuzz.Console;

public static class DemoServiceRegistration
{
    private static readonly Lazy<Type[]> CachedDemoTypes = new(DiscoverDemoTypesCore);

    public static IServiceCollection AddFizzBuzzDemos(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        foreach (var demoType in DiscoverDemoTypes())
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IDemo), demoType));
        }

        return services;
    }

    internal static Type[] DiscoverDemoTypes() => CachedDemoTypes.Value;

    private static Type[] DiscoverDemoTypesCore() =>
        typeof(IDemo).Assembly
            .GetTypes()
            .Where(type =>
                type is { IsClass: true, IsAbstract: false } &&
                !type.IsGenericTypeDefinition &&
                type.IsPublic &&
                typeof(IDemo).IsAssignableFrom(type))
            .Where(type =>
                type.Namespace is not null &&
                type.Namespace.StartsWith("Scott.FizzBuzz.Core.Demos", StringComparison.Ordinal))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .ToArray();
}
