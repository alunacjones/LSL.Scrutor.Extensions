# DecorateWithAsyncInterceptor

This method utilises the `IAsyncInterceptor` interface defined in [Castle.Core.AsyncInterceptor](https://github.com/JSkimming/Castle.Core.AsyncInterceptor).
Please refer to the documentation there to see how to implement an `IAsyncInterceptor`

## Example

Given definitions for the following in an assembly:

> **NOTE**: The following example expects that an `IConsole` implementation is registered in the `IServiceCollection`
> See [LSL.AbstractConsole](https://www.nuget.org/packages/LSL.AbstractConsole) for an implementation that can be used with 
> an `IServiceCollection` via the [LSL.AbstractConsole.ServiceProvider](https://www.nuget.org/packages/LSL.AbstractConsole.ServiceProvider) 
> package.

First we define an `IAsyncInterceptor`

```csharp
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
        _console.WriteLine("Before invocation");
        invocation.Proceed();
        var task = (Task)invocation.ReturnValue;
        await task;

        _console.WriteLine("After Invocation");
    }

    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        // No need to implement as we only have one method with a Task return type
        throw new System.NotImplementedException();
    }

    public void InterceptSynchronous(IInvocation invocation)
    {
        // No need to implement as we only have one method with a Task return type
        throw new System.NotImplementedException();
    }
}
```

We then define a service (and it's interface) with an `async` method to intercept:

```csharp
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
```

We can then register the asynchronous interceptor as follows:

```csharp
services
    .AddInterceptorsFromAssemblyOf<MyAsyncService>()
    .AddScoped<IMyAsyncService, MyAsyncService>()
    .AddAbstractConsole()
    .DecorateWithAsyncInterceptor<IMyAsyncService, MyAsyncInterceptor>();
```

We could also use the async decoration configuration method as follows:

```csharp
services
    .AddInterceptorsFromAssemblyOf<MyAsyncService>()
    .AddScoped<IMyAsyncService, MyAsyncService>()
    .AddAbstractConsole()
    .DecorateWithAsyncInterceptors<IMyAsyncService>(c => c.AddInterceptor<MyAsyncInterceptor>());
```

Both methods of registration end up doing the same thing. The first one is just a convenience method for less code.

A consumer of this async service will then have the interceptor called on the service:

```csharp
public class MyConsumer
{
    private readonly IMyAsyncService _serviceToDecorate;

    public MyConsumer(IMyAsyncService serviceToDecorate) => _serviceToDecorate = serviceToDecorate;

    public async Task DoSomethingElse()
    {
        // This would result in an `IConsole`
        // getting output of:
        // Before invocation
        // My output
        // After invocation
        await _serviceToDecorate.RunAsync();
    }
}
```
<!-- END:HIDE -->