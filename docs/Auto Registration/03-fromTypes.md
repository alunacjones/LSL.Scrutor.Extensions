# Auto registration from types

The following example shows auto registration of types
from the assemblies of the given types:

```csharp { data-fiddle="jfCnxK" }
new ServiceCollection()
    .AutoRegisterServicesFromAssembliesOfTypes(new[] { typeof(Program) });
```