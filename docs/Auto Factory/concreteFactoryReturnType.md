# A concrete factory return type

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

## Using your generated factory

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
