using Castle.DynamicProxy;
using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests;

internal class MyInterceptor : IInterceptor
{
    private readonly IConsole _console;

    public MyInterceptor(IConsole console) => _console = console;

    public void Intercept(IInvocation invocation)
    {
        _console.WriteLine($"Before invoke of {invocation.Method.Name}");
        invocation.Proceed();
        _console.WriteLine($"After invoke of {invocation.Method.Name}");
    }
}