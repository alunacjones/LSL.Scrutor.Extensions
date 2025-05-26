[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-scrutor-extensions.svg)](https://ci.appveyor.com/project/alunacjones/lsl-scrutor-extensions)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.Scrutor.Extensions)](https://coveralls.io/github/alunacjones/LSL.Scrutor.Extensions)
[![NuGet](https://img.shields.io/nuget/v/LSL.Scrutor.Extensions.svg)](https://www.nuget.org/packages/LSL.Scrutor.Extensions/)

# LSL.Scrutor.Extensions

This package provides some extensions to [Scrutor](https://www.nuget.org/packages/scrutor/) and [Microsoft.Extensions.DependencyInjection.Abstractions ](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/)

The current library provides extensions for:

> **NOTE**: Some extension methods use [Castle.Core](https://www.nuget.org/packages/Castle.Core) and therefore the interfaces
> and classes used on each extension method must be public

| Feature                                                                                                                                                                      | Uses Castle.Core? |
| ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------- |
| [Auto Factories](./Auto%20Factories/01-overview.md) | Yes                                                             |
| [Auto Registration](./Auto%20Registration/01-overview.md)                                                              | No                                                              |
| [Intercepting Decorators](./Intercepting%20Decorators/01-overview.md)                                            | Yes                                                             |


