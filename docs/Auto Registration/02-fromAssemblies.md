# Auto registration from assemblies

The following example will automatically register services for
the entry point assembly:

```csharp  { data-fiddle="2SdJop" }
var services = new ServiceCollection()
    .AutoRegisterServices(Assembly.GetEntryAssembly());
```