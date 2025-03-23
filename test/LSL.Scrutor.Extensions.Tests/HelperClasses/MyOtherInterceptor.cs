using Castle.DynamicProxy;
using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

internal class MyOtherInterceptor(IConsole console) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        console.WriteLine("Before invoke (other)");
        invocation.Proceed();
        console.WriteLine("After invoke (other)");
    }
}