# An interface for the factory return type

Assuming you have a definition for a factory interface as below:

```csharp { data-fiddle="JTMDY6" }
public interface IMyFactory
{
    IMyService Create(string name);
}
```

Then we need to further configure our auto factory so that it knows
what concrete type to instantiate. This can be achieved as follows:

```csharp { data-fiddle="JTMDY6" }
services.AddAutoFactory<IMyFactory>(c => c
    .AddConcreteType<IMyService, MyService>()
    .SetLifetime(ServiceLifetime.Scoped))
```

The code above configures the settings for the auto factory
using the delegate we pass into the `AddAutoFactory` call.

In this instance we are also electing to call the optional `SetLifeTime` 
method to set the `ServiceLifetime` for the registered factory.

> **NOTE**: The default lifetime for a factory is `Singleton`
