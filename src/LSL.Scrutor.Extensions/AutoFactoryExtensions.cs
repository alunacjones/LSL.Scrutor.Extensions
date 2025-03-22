using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using LSL.Scrutor.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// AutoFactoryExtensions
/// </summary>
public static class AutoFactoryExtensions
{
    /// <summary>
    /// Adds a factory implementation for <c><typeparamref name="TFactory"/></c>
    /// </summary>
    /// <param name="services">The service collection to add to</param>
    /// <param name="lifetime"></param>
    /// <typeparam name="TFactory">The interface type of the factory</typeparam>
    /// <returns></returns>
    public static IServiceCollection AddAutoFactory<TFactory>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TFactory : class => 
        AddAutoFactory(services, typeof(TFactory), lifetime);

    /// <summary>
    /// Adds a factory implementation for <c><paramref name="factoryInterfaceType"/></c>
    /// </summary>
    /// <param name="services">The service collection to add to</param>
    /// <param name="factoryInterfaceType">The interface type of the factory</param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddAutoFactory(
        this IServiceCollection services,
        Type factoryInterfaceType,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.Add(new(factoryInterfaceType, sp => CreateFactory(sp, factoryInterfaceType), lifetime));
        return services;
    }

    private static object CreateFactory(IServiceProvider serviceProvider, Type factoryInterfaceType) => 
        ProxyGeneratorContainer.ProxyGeneratorInstance.CreateInterfaceProxyWithoutTarget(
            factoryInterfaceType,
            new FactoryInterceptor(serviceProvider)
        );

    private class FactoryInterceptor : IInterceptor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<MethodInfo, ObjectFactory> _factories = new();

        internal FactoryInterceptor(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public void Intercept(IInvocation invocation)
        {
            var factory = _factories.GetOrAdd(invocation.Method, CreateFactory);
            invocation.ReturnValue = factory(_serviceProvider, invocation.Arguments);
        }

        private ObjectFactory CreateFactory(MethodInfo method) => 
            ActivatorUtilities.CreateFactory(
                method.ReturnType,
                [.. method.GetParameters().Select(p => p.ParameterType)]);
    }    
}