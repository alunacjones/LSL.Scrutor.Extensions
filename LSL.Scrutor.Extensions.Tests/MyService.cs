using System.Threading.Tasks;
using Castle.DynamicProxy;
using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests;

public class MyService
{
    private readonly AnotherDependency _other;
    private readonly string _name;
    
    public MyService(string name, AnotherDependency other)
    {
        _name = name;
        _other = other;
    }

    public string Name => _other.FormatName(_name);
}

public interface IMyAsyncService
{
    Task RunAsync();
}

public class MyAsyncService : IMyAsyncService
{
    private readonly IConsole _console;

    public MyAsyncService(IConsole console)
    {
        _console = console;
    }

    public async Task RunAsync()
    {
        await Task.Delay(1000);
        _console.WriteLine("My output");
    }
}

public class MyAsyncInterceptor : IAsyncInterceptor
{
    private readonly IConsole _console;

    public MyAsyncInterceptor(IConsole console)
    {
        _console = console;
    }

    public void InterceptAsynchronous(IInvocation invocation)
    {
        invocation.ReturnValue = InternalInterceptAsynchronous(invocation);    
    }

    private async Task InternalInterceptAsynchronous(IInvocation invocation)
    {
        // Step 1. Do something prior to invocation.

        _console.WriteLine("Before invocation");
        invocation.Proceed();
        var task = (Task)invocation.ReturnValue;
        await task;

        _console.WriteLine("After Invocation");
        // Step 2. Do something after invocation.
    }

    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        throw new System.NotImplementedException();
    }

    public void InterceptSynchronous(IInvocation invocation)
    {
        throw new System.NotImplementedException();
    }
}