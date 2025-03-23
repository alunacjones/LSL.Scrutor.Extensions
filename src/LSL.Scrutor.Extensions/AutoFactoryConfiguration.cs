using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.Scrutor.Extensions;

/// <summary>
/// Used to configure an auto factory
/// </summary>
public class AutoFactoryConfiguration
{
    private readonly Dictionary<Type, Type> _typeMappings = [];

    /// <summary>
    /// Registers a concrete type <c><typeparamref name="TImplementation"/></c>
    /// for <c><typeparamref name="TService"/></c>
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    /// <returns></returns>
    public AutoFactoryConfiguration AddConcreteType<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService => 
        AddConcreteType(typeof(TService), typeof(TImplementation));

    /// <summary>
    /// Registers a concrete type <c><paramref name="implementationType"/></c>
    /// for <c><paramref name="serviceType"/></c>
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="implementationType"></param>
    /// <returns></returns>
    public AutoFactoryConfiguration AddConcreteType(Type serviceType, Type implementationType)
    {
        _typeMappings.Add(serviceType, implementationType);
        return this;
    }

    /// <summary>
    /// Sets the auto factory's <c><see cref="ServiceLifetime"/></c>
    /// </summary>
    /// <remarks>
    /// If this is not called then the factory will be registered
    /// as a <see cref="ServiceLifetime.Singleton"><c>Singleton</c></see>
    /// </remarks>
    /// <param name="serviceLifetime"></param>
    /// <returns></returns>
    public AutoFactoryConfiguration SetLifetime(ServiceLifetime serviceLifetime)
    {
        Lifetime = serviceLifetime;
        return this;
    }

    internal IReadOnlyDictionary<Type, Type> TypeMappings => _typeMappings;
    internal ServiceLifetime Lifetime { get; private set; } = ServiceLifetime.Singleton;
}