using System;
using System.Collections.Generic;

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
        where TImplementation : class, TService        
    {
        _typeMappings.Add(typeof(TService), typeof(TImplementation));
        return this;
    }

    internal IReadOnlyDictionary<Type, Type> TypeMappings => _typeMappings;
}