using System;
using System.Linq;
using FluentAssertions;
using LSL.Scrutor.Extensions.Tests.AutoRegisterTestClasses;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.Scrutor.Extensions.Tests;

public static class ServiceCollectionAssertionExtensions
{
    public static IServiceCollection ShouldHaveAllTheServicesRegistered(
        this IServiceCollection services,
        int expectedCount = 3,
        Action<IServiceCollection> additionalAssertionsAction = null)
    {
        services.Should().HaveCount(expectedCount);

        services
            .ShouldHaveARegistrationOf<ITransientService, TransientService>(ServiceLifetime.Transient)
            .ShouldHaveARegistrationOf<IScopedService, ScopedService>(ServiceLifetime.Scoped)
            .ShouldHaveARegistrationOf<ISingletonService, SingletonService>(ServiceLifetime.Singleton);

        additionalAssertionsAction?.Invoke(services);

        return services;
    }

    public static IServiceCollection ShouldHaveARegistrationOf<TServiceType, TImplementationType>(
        this IServiceCollection source,
        ServiceLifetime expectedServiceLifetime)
    {
        var descriptor = source.Where(sd => sd.ServiceType == typeof(TServiceType))
            .Single();

        descriptor.ImplementationType
            .Should()
            .BeAssignableTo<TImplementationType>();

        descriptor.Lifetime.Should().Be(expectedServiceLifetime);

        return source;
    }
}