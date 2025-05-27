# Overview of auto registration

The auto registration methods will automatically register all concrete classes that
implement one of the following interfaces:

| Interface           | Registered Lifetime         |
| ------------------- | --------------------------- |
| `IScopedService`    | `ServiceLifetime.Scoped`    |
| `ISingletonService` | `ServiceLifetime.Singleton` |
| `ITransientService` | `ServiceLifetime.Transient` |

!!! note
    All of these interfaces live in the `LSL.Scrutor.Extensions` namespace.

!!! warning
    If a concrete type has multiple lifetime interfaces then an `ArgumentException` will be thrown
    with a message that tells you of the problematic class that needs fixing