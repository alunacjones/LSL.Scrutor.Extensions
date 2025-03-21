using System.IO;
using FluentAssertions;
using FluentAssertions.Execution;
using LSL.AbstractConsole.ServiceProvider;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.Scrutor.Extensions.Tests;

public class DecorateWithInterceptorTests
{
    [Test]
    public void DecorateWithInterceptor()
    {        
        // Arrange
        var writer = new StringWriter();

        var sp = new ServiceCollection()
            .AddInterceptorsFromAssemblyOf<DecorateWithInterceptorTests>()
            .AddScoped<IThingy, Thingy>()
            .AddAbstractConsole(c => c.TextWriter = writer)
            .DecorateWithInterceptor<IThingy, MyInterceptor>()
            .BuildServiceProvider();

        var interceptedSut = sp.GetRequiredService<IThingy>();

        // Act
        interceptedSut.DoSomething();
        
        // Assert
        using var assertionScope = new AssertionScope();

        var result = writer.ToString();

        result.Should().Be(
            """
            Before invoke of DoSomething
            Something done
            After invoke of DoSomething

            """.ReplaceLineEndings()
        );
    }
}