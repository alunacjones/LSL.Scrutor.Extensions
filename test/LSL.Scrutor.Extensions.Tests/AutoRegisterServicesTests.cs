using System.Linq;
using LSL.Scrutor.Extensions.Tests.AutoRegisterTestClasses;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.Scrutor.Extensions.Tests;

public class AutoRegisterServicesTests
{
    [Test]
    public void WhenRegisteringFromEnumerableAssemblies_ThenTheCollectionShouldHaveTheExpectedServices()
    {
        new ServiceCollection()
            .AutoRegisterServices(new[] { typeof(AutoRegisterServicesTests).Assembly }.AsEnumerable())
            .ShouldHaveAllTheServicesRegistered();
    }

    [Test]
    public void WhenRegisteringFromEnumerableAssembliesWithAdditionalConfiguration_ThenTheCollectionShouldHaveTheExpectedServices()
    {
        new ServiceCollection()
            .AutoRegisterServices(
                new[] { typeof(AutoRegisterServicesTests).Assembly }.AsEnumerable(),
                s => s
                    .AddClasses(t => t.AssignableTo<ExtraService>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime())
            .ShouldHaveAllTheServicesRegistered(4, s => s.ShouldHaveARegistrationOf<ExtraService, ExtraService>(ServiceLifetime.Singleton));
    }

    [Test]
    public void WhenRegisteringFromParamsAssemblies_ThenTheCollectionShouldHaveTheExpectedServices()
    {
        new ServiceCollection()
            .AutoRegisterServices([typeof(AutoRegisterServicesTests).Assembly])
            .ShouldHaveAllTheServicesRegistered();
    }

    [Test]
    public void WhenRegisteringFromParamsAssembliesWithAdditionalConfiguration_ThenTheCollectionShouldHaveTheExpectedServices()
    {
        new ServiceCollection()
            .AutoRegisterServices(
                s => s
                    .AddClasses(t => t.AssignableTo<ExtraService>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime(),
                [typeof(AutoRegisterServicesTests).Assembly]
            )
            .ShouldHaveAllTheServicesRegistered(4, s => s.ShouldHaveARegistrationOf<ExtraService, ExtraService>(ServiceLifetime.Singleton));
    }

    [Test]
    public void WhenRegisteringFromTypes_ThenTheCollectionShouldHaveTheExpectedServices()
    {
        new ServiceCollection()
            .AutoRegisterServicesFromAssembliesOfTypes([typeof(AutoRegisterServicesTests)])
            .ShouldHaveAllTheServicesRegistered();
    }

    [Test]
    public void WhenRegisteringFromTypesWithAdditionalConfiguration_ThenTheCollectionShouldHaveTheExpectedServices()
    {
        new ServiceCollection()
            .AutoRegisterServicesFromAssembliesOfTypes(
                [typeof(AutoRegisterServicesTests)],
                s => s
                    .AddClasses(t => t.AssignableTo<ExtraService>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime())
            .ShouldHaveAllTheServicesRegistered(4, s => s.ShouldHaveARegistrationOf<ExtraService, ExtraService>(ServiceLifetime.Singleton));
    }

    [Test]
    public void WhenRegisteringFromAType_ThenTheCollectionShouldHaveTheExpectedServices()
    {
        new ServiceCollection()
            .AutoRegisterServicesFromAssemblyOf<AutoRegisterServicesTests>()
            .ShouldHaveAllTheServicesRegistered();
    }

    [Test]
    public void WhenRegisteringFromATypeWithAdditionalConfiguration_ThenTheCollectionShouldHaveTheExpectedServices()
    {
        new ServiceCollection()
            .AutoRegisterServicesFromAssemblyOf<AutoRegisterServicesTests>(
                s => s
                    .AddClasses(t => t.AssignableTo<ExtraService>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime()
            )
            .ShouldHaveAllTheServicesRegistered(4, s => s.ShouldHaveARegistrationOf<ExtraService, ExtraService>(ServiceLifetime.Singleton));
    }
}
