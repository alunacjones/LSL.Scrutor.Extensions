using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using LSL.AbstractConsole.ServiceProvider;
using LSL.Scrutor.Extensions.Tests.HelperClasses;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.Scrutor.Extensions.Tests;

public class DecorateWithInterceptorTests
{
    [Test]
    public void DecorateWithInvalidInterceptor_ShouldThrowAnException()
    {
        // Arrange
        var writer = new StringWriter();

        var sp = new ServiceCollection()
            .AddInterceptorsFromAssemblyOf<DecorateWithInterceptorTests>()
            .AddScoped<ISyncServiceToDecorate, SyncServiceToDecorate>()
            .AddAbstractConsole(c => c.TextWriter = writer)
            .DecorateWithInterceptors(typeof(ISyncServiceToDecorate), typeof(object))
            .BuildServiceProvider();

        // Act & Assert
        new Action(() => sp.GetRequiredService<ISyncServiceToDecorate>()).Should().Throw<ArgumentException>();
    }

    [Test]
    public void DecorateWithInterceptor()
    {        
        // Arrange
        var writer = new StringWriter();

        var sp = new ServiceCollection()
            .AddInterceptorsFromAssemblyOf<DecorateWithInterceptorTests>()
            .AddScoped<ISyncServiceToDecorate, SyncServiceToDecorate>()
            .AddAbstractConsole(c => c.TextWriter = writer)
            .DecorateWithInterceptor<ISyncServiceToDecorate, MyInterceptor>()
            .DecorateWithInterceptor<ISyncServiceToDecorate, MyOtherInterceptor>()
            .BuildServiceProvider();

        var interceptedSut = sp.GetRequiredService<ISyncServiceToDecorate>();

        // Act
        interceptedSut.DoSomething();
        
        // Assert
        using var assertionScope = new AssertionScope();

        var result = writer.ToString();

        result.Should().Be(
            """
            Before invoke (other)
            Before invoke of DoSomething
            Something done
            After invoke of DoSomething
            After invoke (other)

            """.ReplaceLineEndings()
        );
    }

    [Test]
    public void DecorateWithInterceptors()
    {        
        // Arrange
        var writer = new StringWriter();

        var sp = new ServiceCollection()
            .AddInterceptorsFromAssemblyOf<DecorateWithInterceptorTests>()
            .AddScoped<ISyncServiceToDecorate, SyncServiceToDecorate>()
            .AddAbstractConsole(c => c.TextWriter = writer)
            .DecorateWithInterceptors<ISyncServiceToDecorate>(new[] { typeof(MyInterceptor), typeof(MyOtherInterceptor) }.AsEnumerable())
            .BuildServiceProvider();

        var interceptedSut = sp.GetRequiredService<ISyncServiceToDecorate>();

        // Act
        interceptedSut.DoSomething();
        
        // Assert
        using var assertionScope = new AssertionScope();

        var result = writer.ToString();

        result.Should().Be(
            """
            Before invoke (other)
            Before invoke of DoSomething
            Something done
            After invoke of DoSomething
            After invoke (other)

            """.ReplaceLineEndings()
        );
    }

    [Test]
    public void DecorateWithInterceptorsViaConfiguration()
    {        
        // Arrange
        var writer = new StringWriter();

        var sp = new ServiceCollection()
            .AddInterceptorsFromAssemblyOf<DecorateWithInterceptorTests>()
            .AddScoped<ISyncServiceToDecorate, SyncServiceToDecorate>()
            .AddAbstractConsole(c => c.TextWriter = writer)
            .DecorateWithInterceptors<ISyncServiceToDecorate>(c => c
                .Add<MyInterceptor>()
                .Add<MyOtherInterceptor>())
            .BuildServiceProvider();

        var interceptedSut = sp.GetRequiredService<ISyncServiceToDecorate>();

        // Act
        interceptedSut.DoSomething();
        
        // Assert
        using var assertionScope = new AssertionScope();

        var result = writer.ToString();

        result.Should().Be(
            """
            Before invoke (other)
            Before invoke of DoSomething
            Something done
            After invoke of DoSomething
            After invoke (other)

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

    [Test]
    public async Task DecorateWithAsyncInterceptorsViaConfiguration()
    {
        // Arrange
        var writer = new StringWriter();

        var sp = new ServiceCollection()
            .AddInterceptorsFromAssemblyOf<DecorateWithInterceptorTests>()
            .AddScoped<IMyAsyncService, MyAsyncService>()
            .AddAbstractConsole(c => c.TextWriter = writer)
            .DecorateWithAsyncInterceptors<IMyAsyncService>(c => c.Add<MyAsyncInterceptor>())
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

    [Test]
    public void DecorateWithAsyncInterceptorsWithNoInterceptors_ShouldThrowAnArgumentException()
    {
        // Arrange, Act & Assert
        new Action(() => new ServiceCollection()
            .AddInterceptorsFromAssemblyOf<DecorateWithInterceptorTests>()
            .AddScoped<IMyAsyncService, MyAsyncService>()
            .DecorateWithAsyncInterceptors<IMyAsyncService>(c => { /* Add nothing */ })
            .BuildServiceProvider()
        )
        .Should().Throw<ArgumentException>();
    }        
}