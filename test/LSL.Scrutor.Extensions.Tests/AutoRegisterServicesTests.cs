using System;
using System.Linq;
using FluentAssertions;
using LSL.Scrutor.Extensions.Tests.AutoRegisterTestClasses;
using Microsoft.Extensions.DependencyInjection;
using MultipleLifeTimesAssembly;

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
    public void WhenRegisteringAClassThatHasMultipleLifetimes_ThenItShouldThrowAnException()
    {
        new Action(() => new ServiceCollection()
            .AutoRegisterServicesFromAssemblyOf<MultipleLifetimes>()
        )
        .Should()
        .ThrowExactly<ArgumentException>()
        .WithMessage("Type MultipleLifeTimesAssembly.MultipleLifetimes implements many lifetime interfaces. You may only use one of IScopedService, ITransientService or ISingletonService");
    }

    [Test]
    public void WhenRegisteringFromEnumerableWithNullAssemblies_ThenItShouldThrow()
    {
        new Action(() => new ServiceCollection()
            .AutoRegisterServices(
                null,
                s => s
                    .AddClasses(t => t.AssignableTo<ExtraService>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime())
        )
        .Should()
        .ThrowExactly<ArgumentNullException>()
        .And
        .ParamName.Should().Be("assembliesToScan");
    }

    [Test]
    public void WhenRegisteringFromParamsWithEmptyAssemblies_ThenItShouldThrow()
    {
        new Action(() => new ServiceCollection()
            .AutoRegisterServices())
        .Should()
        .ThrowExactly<ArgumentException>()
        .And
        .ParamName.Should().Be("assembliesToScan");
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
