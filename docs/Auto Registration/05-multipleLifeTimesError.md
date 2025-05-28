# Auto registration with multiple lifetimes

The following example shows auto registration failing due to a class having multiple lifetimes:

```csharp { data-fiddle="B6t9qK" }
new ServiceCollection()
    .AutoRegisterServicesFromAssemblyOf<Program>();
```

The dependent interfaces and classes are defined as follows:

```csharp { data-fiddle="B6t9qK" }
public interface IMyClass
{
    void SayHello(string name);
}

public class MyClass : IMyClass, IScopedService, ISingletonService
{
    public void SayHello(string name) => Console.WriteLine($"Hello {name}");
}
```
