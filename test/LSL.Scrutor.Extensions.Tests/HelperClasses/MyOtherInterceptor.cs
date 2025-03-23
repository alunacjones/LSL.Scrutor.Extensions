using Castle.DynamicProxy;
using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

internal class MyOtherInterceptor : IInterceptor
{
    private readonly IConsole _console;

    public MyOtherInterceptor(IConsole console)
    {
        _console = console;
    }

    public void Intercept(IInvocation invocation)
    {
        _console.WriteLine("Before invoke (other)");
        invocation.Proceed();
        _console.WriteLine("After invoke (other)");
    }
}