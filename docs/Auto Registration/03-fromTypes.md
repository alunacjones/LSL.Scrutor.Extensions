# Auto registration from types

The following example shows auto registration of types
from the assemblies of the given types:

```csharp { data-fiddle="rh71PB" }
new ServiceCollection()
    .AutoRegisterServicesFromAssembliesOfTypes(new[] { typeof(Program) });
```