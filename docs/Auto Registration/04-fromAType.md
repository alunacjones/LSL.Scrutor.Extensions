# Auto registration from a type

The following example shows auto registration of types
from the assembly of the given type:

```csharp { data-fiddle="VRMZEM" }
new ServiceCollection()
    .AutoRegisterServicesFromAssemblyOf<AutoRegisterServicesTests>();
```