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
            .AddAutoFactory<IMyOtherFactory>()
            .AddScoped<AnotherDependency>()
            .BuildServiceProvider();

        sp.GetRequiredService<IMyOtherFactory>().Create("my-name").Name.Should().Be("Prefix: my-name");
    }
}