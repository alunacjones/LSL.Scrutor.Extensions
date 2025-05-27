# Auto registration with multiple lifetimes

The following example shows auto registration of types
from the assembly of the given type:

```csharp { data-fiddle="B6t9qK" }
new ServiceCollection()
    .AutoRegisterServicesFromAssemblyOf<Program>();
```