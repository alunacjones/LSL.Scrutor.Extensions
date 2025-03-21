using System.IO;
using System.Threading.Tasks;
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

    [Test]
    public async Task DecorateWithAsyncInterceptor()
    {
        // Arrange
        var writer = new StringWriter();

        var sp = new ServiceCollection()
            .AddInterceptorsFromAssemblyOf<DecorateWithInterceptorTests>()
            .AddScoped<IMyAsyncService, MyAsyncService>()
            .AddAbstractConsole(c => c.TextWriter = writer)
            .DecorateWithAsyncInterceptor<IMyAsyncService, MyAsyncInterceptor>()
            .BuildServiceProvider();

        var interceptedSut = sp.GetRequiredService<IMyAsyncService>();

        // Act
        await interceptedSut.RunAsync();
        
        // Assert
        using var assertionScope = new AssertionScope();

        var result = writer.ToString();

        result.Should().Be(
            """
            Before invocation
            My output
            After Invocation

            """.ReplaceLineEndings()
        );
    }
}