# Auto registration from a type

The following example shows auto registration of types
from the assemblies of the given types:

```csharp { data-fiddle="VRMZEM" }
new ServiceCollection()
    .AutoRegisterServicesFromAssemblyOf<AutoRegisterServicesTests>();
```