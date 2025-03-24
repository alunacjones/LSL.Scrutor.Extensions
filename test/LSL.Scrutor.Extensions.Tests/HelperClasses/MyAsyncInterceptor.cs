using System.Threading.Tasks;
using Castle.DynamicProxy;
using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

public class MyAsyncInterceptor(IConsole console) : IAsyncInterceptor
{
    public void InterceptAsynchronous(IInvocation invocation) => invocation.ReturnValue = InternalInterceptAsynchronous(invocation);

    private async Task InternalInterceptAsynchronous(IInvocation invocation)
    {
        console.WriteLine("Before invocation");
        invocation.Proceed();
        var task = (Task)invocation.ReturnValue;
        await task;

        console.WriteLine("After Invocation");
    }

    public void InterceptAsynchronous<TResult>(IInvocation invocation) =>
        // No need to implement as we only have one method with a Task return type
        throw new System.NotImplementedException();

    public void InterceptSynchronous(IInvocation invocation) =>
        // No need to implement as we only have one method with a Task return type
        throw new System.NotImplementedException();
}