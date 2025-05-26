using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LSL.Scrutor.Extensions;
using Scrutor;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// AutoRegistrationServiceCollectionExtensions
/// </summary>
public static class AutoRegistrationServiceCollectionExtensions
{
    /// <summary>
    /// Automatically registers all concrete types
    /// that implement one of <see cref="ITransientService"/>,
    /// <see cref="IScopedService"/> or
    /// <see cref="ISingletonService"/>
    /// from the provided assemblies
    /// </summary>
    /// <param name="source">The service collection to add registrations to</param>
    /// <param name="assembliesToScan">The assemblies to scan</param>
    /// <param name="extraRegistrationConfigurator">An optional delegate for any custom auto-registration</param>
    /// <returns></returns>
    public static IServiceCollection AutoRegisterServices(
        this IServiceCollection source,
        IEnumerable<Assembly> assembliesToScan,
        Action<IImplementationTypeSelector> extraRegistrationConfigurator = null)
    {
        source.GuardAgainstNull(nameof(source)).Scan(scan =>
        {
            var selector = scan.FromAssemblies(assembliesToScan.GuardAgainstNull(nameof(assembliesToScan)));

            selector
                .AddClasses(classes => classes.AssignableTo<ITransientService>())
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo<IScopedService>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime();

            extraRegistrationConfigurator?.Invoke(selector);
        });

        return source;
    }

    /// <summary>
    /// Automatically registers all concrete types
    /// that implement one of <see cref="ITransientService"/>,
    /// <see cref="IScopedService"/> or
    /// <see cref="ISingletonService"/>
    /// from the provided assemblies
    /// </summary>
    /// <param name="source"></param>
    /// <param name="assembliesToScan"></param>
    /// <returns></returns>
    public static IServiceCollection AutoRegisterServices(
        this IServiceCollection source,
        params Assembly[] assembliesToScan) =>
        source.AutoRegisterServices(assembliesToScan.GuardAgainstEmpty(nameof(assembliesToScan)).AsEnumerable(), null);

    /// <summary>
    /// Automatically registers all concrete types
    /// that implement one of <see cref="ITransientService"/>,
    /// <see cref="IScopedService"/> or
    /// <see cref="ISingletonService"/>
    /// from the provided assemblies
    /// </summary>
    /// <param name="source"></param>
    /// <param name="extraRegistrationConfigurator"></param>
    /// <param name="assembliesToScan"></param>
    /// <returns></returns>
    public static IServiceCollection AutoRegisterServices(
        this IServiceCollection source,
        Action<IImplementationTypeSelector> extraRegistrationConfigurator = null,
        params Assembly[] assembliesToScan) =>
        source.AutoRegisterServices(assembliesToScan.GuardAgainstEmpty(nameof(assembliesToScan)).AsEnumerable(), extraRegistrationConfigurator);

    /// <summary>
    /// Automatically registers all concrete types
    /// that implement one of <see cref="ITransientService"/>,
    /// <see cref="IScopedService"/> or
    /// <see cref="ISingletonService"/>
    /// from the assemblies of the provided types
    /// </summary>
    /// <param name="source"></param>
    /// <param name="types"></param>
    /// <param name="extraRegistrationConfigurator"></param>
    /// <returns></returns>
    public static IServiceCollection AutoRegisterServicesFromAssembliesOfTypes(
        this IServiceCollection source,
        IEnumerable<Type> types,
        Action<IImplementationTypeSelector> extraRegistrationConfigurator = null) =>
        source.AutoRegisterServices(types.Select(t => t.Assembly), extraRegistrationConfigurator);

    /// <summary>
    /// Automatically registers all concrete types
    /// that implement one of <see cref="ITransientService"/>,
    /// <see cref="IScopedService"/> or
    /// <see cref="ISingletonService"/>
    /// from the assembly of <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="extraRegistrationConfigurator"></param>
    /// <returns></returns>
    public static IServiceCollection AutoRegisterServicesFromAssemblyOf<T>(
        this IServiceCollection source,
        Action<IImplementationTypeSelector> extraRegistrationConfigurator = null) =>
        source.AutoRegisterServices([typeof(T).Assembly], extraRegistrationConfigurator);
}