using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.Scrutor.Extensions.Tests;

public class AddAutoFactoryTests
{
    [Test]
    public void AddAutoFactory_GivenAnInterface_ThenGeneratedProxyShouldCreateTheInitialisedService()
    {
        var sp = new ServiceCollection()
            .AddAutoFactory<IMyFactory>()
            .AddScoped<AnotherDependency>()
            .BuildServiceProvider();

        sp.GetRequiredService<IMyFactory>().Create("my-name").Name.Should().Be("Prefix: my-name");
    }
}