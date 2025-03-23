---
outputFileName: index.html
---

[![GitHub Actions Workflow Status](https://github.com/alunacjones/LSL.Scrutor.Extensions/actions/workflows/github-actions.yml/badge.svg)](https://github.com/alunacjones/LSL.Scrutor.Extensions/actions/workflows/github-actions.yml)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.Scrutor.Extensions)](https://coveralls.io/github/alunacjones/LSL.Scrutor.Extensions)
[![NuGet](https://img.shields.io/nuget/v/LSL.Scrutor.Extensions.svg)](https://www.nuget.org/packages/LSL.Scrutor.Extensions/)

# LSL.Scrutor.Extensions

More documentation can be found [here](https://alunacjones.github.io/LSL.Scrutor.Extensions/)

This package providers some extensions to [Scrutor](https://www.nuget.org/packages/scrutor/) and [Microsoft.Extensions.DependencyInjection.Abstractions ](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/)

> **NOTE**: These extension methods use [Castle.Core](https://www.nuget.org/packages/Castle.Core) and therefore the interfaces
> and classes used on each extension method must be public

## AddAutoFactory

If you have a factory interface then you can automatically create a proxy that will instantiate the type with this method.

> **NOTE**: The return type can be an interface as long as it is registered to a concrete type

### Example

Assuming you have a definition for a factory interface as below:

```csharp
public interface IMyFactory
{
    MyService Create(string name);
}
```

and also a class definition of:

```csharp
public class MyService
{
    private readonly string _name;

    public MyService(string name)
    {
        _name = name;
    }

    public void MakeTheServiceDoSomething()
    {
        Console.WriteLine(name);
    }
}
```

Then a factory implementation can be created with the following:

```csharp
services.AddAutoFactory<IMyFactory>()
```

> **NOTE**: The returned service from an `AddAutoFactory`'s 
> interface can
> have other  dependencies too as they will be automatically resolved.
> Any dependencies must be registered in the `IServiceCollection`

This factory can than be injected into other services to create instances
of `MyService` using the factory interface.

#### Using your generated factory

```csharp
public class MyConsumer
{
    private readonly _factory;

    public MyConsumer(IMyFactory factory)
    {
        _factory = factory;
    }

    public void DoSomething()
    {
        // Will write 'a-name' to the console as per the earlier
        // definition of 'MyService'
        _factory.Create("a-name").MakeTheServiceDoSomething();
    }
}
```

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
public interface IThingy
{
    void DoSomething();
}

public class Thingy : IThingy
{
    private readonly IConsole _console;

    public Thingy(IConsole console) => _console = console;

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
    .AddScoped<IThingy, Thingy>()
    .DecorateWithInterceptor<IThingy, MyInterceptor>();
```

Now we can just inject an `IThingy` and get `MyInterceptor` to intercept every call on it.

If using the aforementioned `IConsole` implemtation, then a class that consumes `IThingy`
as shown below would get intercepted:

```csharp
public class MyConsumer
{
    public MyConsumer(IThingy thing) => _thingy = thingy;

    public void DoSomethingElse()
    {
        // This would result in an `IConsole`
        // getting output of:
        // Before invoke of 'DoSomething'
        // Something done
        // After invoke of 'DoSomething'
        _thingy.DoSomething();
    }
}
```