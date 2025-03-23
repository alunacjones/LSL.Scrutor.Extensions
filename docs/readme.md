---
outputFileName: index.html
_layout: landing
---
[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-scrutor-extensions.svg)](https://ci.appveyor.com/project/alunacjones/lsl-scrutor-extensions)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.Scrutor.Extensions)](https://coveralls.io/github/alunacjones/LSL.Scrutor.Extensions)
[![NuGet](https://img.shields.io/nuget/v/LSL.Scrutor.Extensions.svg)](https://www.nuget.org/packages/LSL.Scrutor.Extensions/)

# LSL.Scrutor.Extensions

More documentation can be found [here](https://alunacjones.github.io/LSL.Scrutor.Extensions/)

This package providers some extensions to [Scrutor](https://www.nuget.org/packages/scrutor/) and [Microsoft.Extensions.DependencyInjection.Abstractions ](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/)

> **NOTE**: These extension methods use [Castle.Core](https://www.nuget.org/packages/Castle.Core) and therefore the interfaces
> and classes used on each extension method must be public

## AddAutoFactory

If you have a factory interface then you can automatically create a proxy that will instantiate the type with this method.

### Example with a concrete factory return type

Assuming you have a definition for a factory interface as below:

```csharp
public interface IMyFactoryForaConcreteType
{
    MyService Create(string name);
}
```

and also a class definition of:

```csharp
public class MyService : IMyService
{
    private readonly string _name;
    
    public MyService(string name)
    {
        _name = name;
    }

    public string Name => _name.ToUpper();
}
```

Then a factory implementation can be created with the following:

```csharp
services.AddAutoFactory<IMyFactoryForaConcreteType>();
```

> **NOTE**: The returned service from an `AddAutoFactory`'s 
> interface can
> have other  dependencies too as they will be automatically resolved.
>
> Any dependencies must be registered in the `IServiceCollection`

This factory can than be injected into other services to create instances
of `MyService` using the factory interface.

#### Using your generated factory

```csharp
public class MyConsumer
{
    private readonly IMyFactoryForaConcreteType _factory;

    public MyConsumer(IMyFactoryForaConcreteType factory)
    {
        _factory = factory;
    }

    public void DoSomething()
    {
        // `name` will have the value `A-NAME`
        var name = _factory.Create("a-name").Name;
    }
}
```

### Example with an interface for the factory return type

Assuming you have a definition for a factory interface as below:

```csharp
public interface IMyFactory
{
    IMyService Create(string name);
}
```

Then we need to further configure our auto factory so that it knows
what concrete type to instantiate. This can be achieved as follows:

```csharp
services.AddAutoFactory<IMyFactory>(c => c
    .AddConcreteType<IMyService, MyService>()
    .SetLifetime(ServiceLifetime.Scoped))
```

The code above configures the settings for the auto factory
using the delegate we pass into the `AddAutoFactory` call.

In this instance we are also electing to call the optional `SetLifeTime` 
method to set the `ServiceLifetime` for the registered factory.

> **NOTE**: The default lifetime for a factory is `Singleton`

## DecorateWithInterceptor

Scrutor provides great `Decorator` functionality but sometimes a more [Aspect Oreinted Programming](https://www.google.com/search?client=firefox-b-d&q=aspect+oriented+programming) paradigm is needed.

[Castle.Core](https://www.nuget.org/packages/castle.core/) provides great interception capabilites that this method utilises.

### Example

Given definitions for the following in an assembly:

> **NOTE**: The following example expectes that an `IConsole` implementation is registered in the `IServiceCollection`
> See [LSL.AbstractConsole](https://www.nuget.org/packages/LSL.AbstractConsole) for an implementation that can be used with 
> an `IServiceCollection` via the [LSL.AbstractConsole.ServiceProvider](https://www.nuget.org/packages/LSL.AbstractConsole.ServiceProvider) 
> package.

```csharp
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

```csharp
services.AddInterceptorsFromAssemblyOf<MyInterceptor>()
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

    public MyConsumer(ISyncServiceToDecorate syncServiceToDecorate) => _syncServiceToDecorate = syncServiceToDecorate;

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

### Example with multiple interceptors

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
services.AddInterceptorsFromAssemblyOf<MyInterceptor>()
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

## DecorateWithAsyncInterceptor

This method utilises the `IAsyncInterceptor` interface defined in [Castle.Core.AsyncInterceptor](https://github.com/JSkimming/Castle.Core.AsyncInterceptor).
Please refer to the documentation there to see how to implement an `IAsyncInterceptor`

### Example

Given definitions for the following in an assembly:

> **NOTE**: The following example expectes that an `IConsole` implementation is registered in the `IServiceCollection`
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

We could also use the async decoratition configuration method as follows:

```
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
