using System;
using FluentAssertions;
using LSL.Scrutor.Extensions.Tests.HelperClasses;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.Scrutor.Extensions.Tests;

public class AddAutoFactoryTests
{
    [Test]
    public void AddAutoFactory_GivenAnInterface_ThenGeneratedProxyShouldCreateTheInitialisedService()
    {
        var sp = new ServiceCollection()
            .AddAutoFactory<IMyFactoryForaConcreteType>()
            .AddScoped<Prefixer>()
            .BuildServiceProvider();
        
        sp.GetRequiredService<IMyFactoryForaConcreteType>().Create("my-name").Name.Should().Be("Prefix: my-name");
    }

    [Test]
    public void AddAutoFactory_GivenAnInterface_ThenGeneratedProxyShouldCreateTheInitialisedServiceViaAnInterface()
    {
        var sp = new ServiceCollection()
            .AddAutoFactory<IMyFactory>(c => c
                .AddConcreteType<IMyService, MyService>()
                .SetLifetime(ServiceLifetime.Scoped))
            .AddScoped<Prefixer>()
            .BuildServiceProvider();

        sp.GetRequiredService<IMyFactory>().Create("my-name").Name.Should().Be("Prefix: my-name");
    }

    [Test]
    public void AddAutoFactory_GivenAnInterface_ThenItShouldThrowTheExpectedExceptionIfNoConcreteTypeIsProvided()
    {
        var sp = new ServiceCollection()
            .AddAutoFactory<IMyFactory>()
            .AddScoped<Prefixer>()
            .BuildServiceProvider();

        new Action(() => sp.GetRequiredService<IMyFactory>().Create("my-name")).Should().Throw<InvalidOperationException>();
    }    
}