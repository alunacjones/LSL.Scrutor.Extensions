using Castle.DynamicProxy;
using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

internal class MyInterceptor(IConsole console) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        console.WriteLine($"Before invoke of {invocation.Method.Name}");
        invocation.Proceed();
        console.WriteLine($"After invoke of {invocation.Method.Name}");
    }
}