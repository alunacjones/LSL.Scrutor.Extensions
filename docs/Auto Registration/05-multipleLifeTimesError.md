# Auto registration with multiple lifetimes

The following example shows auto registration failing due to a class having multiple lifetimes:

```csharp { data-fiddle="B6t9qK" }
new ServiceCollection()
    .AutoRegisterServicesFromAssemblyOf<Program>();
```