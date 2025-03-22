using System.Threading.Tasks;
using Castle.DynamicProxy;
using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

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