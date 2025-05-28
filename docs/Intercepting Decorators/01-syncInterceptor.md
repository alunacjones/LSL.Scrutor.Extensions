# DecorateWithInterceptor

## Example

Given definitions for the following in an assembly:

> **NOTE**: The following example expects that an `IConsole` implementation is registered in the `IServiceCollection`
> See [LSL.AbstractConsole](https://www.nuget.org/packages/LSL.AbstractConsole) for an implementation that can be used with 
> an `IServiceCollection` via the [LSL.AbstractConsole.ServiceProvider](https://www.nuget.org/packages/LSL.AbstractConsole.ServiceProvider) 
> package.

```csharp { data-fiddle="5My3fJ" }
public interface ISyncServiceToDecorate
{
    void DoSomething();
}

public class SyncServiceToDecorate : ISyncServiceToDecorate
{
    private readonly IConsole _console;

    public SyncServiceToDecorate(IConsole console) => _console = console;

    public void DoSomething() => _console.WriteLine("Something done");
}

public class MyInterceptor : IInterceptor
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
```

Then we can easily register a decorator with the provided interceptor as follows:

```csharp { data-fiddle="5My3fJ" }
services
    .AddInterceptorsFromAssemblyOf<MyInterceptor>()
    .AddAbstractConsole()
    .AddScoped<ISyncServiceToDecorate, SyncServiceToDecorate>()
    .DecorateWithInterceptor<ISyncServiceToDecorate, MyInterceptor>();
```

Now we can just inject an `ISyncServiceToDecorate` and get `MyInterceptor` to intercept every call on it.

If using the aforementioned `IConsole` implementation, then a class that consumes `ISyncServiceToDecorate`
as shown below would get intercepted:

```csharp
public class MyConsumer
{
    private readonly ISyncServiceToDecorate _syncServiceToDecorate;

    public MyConsumer(ISyncServiceToDecorate syncServiceToDecorate) => 
        _syncServiceToDecorate = syncServiceToDecorate;

    public void DoSomethingElse()
    {
        // This would result in an `IConsole`
        // getting output of:
        // Before invoke of 'DoSomething'
        // Something done
        // After invoke of 'DoSomething'
        _syncServiceToDecorate.DoSomething();
    }
}
```

## Example with multiple interceptors

Using the classes in the previous example we can add a new interceptor to show 
registration of multiple interceptors using a configuration delegate:

```csharp
// Extra interceptor
public class MyOtherInterceptor : IInterceptor
{
    private readonly IConsole _console;

    public MyInterceptor(IConsole console) => _console = console;

    public void Intercept(IInvocation invocation)
    {
        _console.WriteLine($"(Other) Before invoke of {invocation.Method.Name}");
        invocation.Proceed();
        _console.WriteLine($"(Other) After invoke of {invocation.Method.Name}");
    }
}
```

The following code will then register both interceptors against our service:

```csharp
services
    .AddInterceptorsFromAssemblyOf<MyInterceptor>()
    .AddAbstractConsole()
    .AddScoped<ISyncServiceToDecorate, SyncServiceToDecorate>()
    .DecorateWithInterceptor<ISyncServiceToDecorate>(c => c
        .AddInterceptor<MyInterceptor>()
        .AddInterceptor<MyOtherInterceptor>());
```

Now the following consumer code will result in extra logging:

```csharp
public class MyConsumer
{
    private readonly ISyncServiceToDecorate _syncServiceToDecorate;

    public MyConsumer(ISyncServiceToDecorate syncServiceToDecorate) => _syncServiceToDecorate = syncServiceToDecorate;

    public void DoSomethingElse()
    {
        // This would result in an `IConsole`
        // getting output of:
        // (Other) Before invoke of 'DoSomething'
        // Before invoke of 'DoSomething'
        // Something done
        // After invoke of 'DoSomething'
        // (Other) After invoke of 'DoSomething'
        _syncServiceToDecorate.DoSomething();
    }
}
```
