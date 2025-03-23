using System;
using System.Collections;
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
    /// <param name="configurator">An optional configurator for configuring the auto factory further</param>
    /// <typeparam name="TFactory">The interface type of the factory</typeparam>
    /// <returns></returns>
    /// <example lang="csharp">
    /// try this
    /// </example>
    public static IServiceCollection AddAutoFactory<TFactory>(
        this IServiceCollection services,
        Action<AutoFactoryConfiguration> configurator = null)
        where TFactory : class => 
        AddAutoFactory(services, typeof(TFactory), configurator);

    /// <summary>
    /// Adds a factory implementation for <c><paramref name="factoryInterfaceType"/></c>
    /// </summary>
    /// <param name="services">The service collection to add to</param>
    /// <param name="factoryInterfaceType">The interface type of the factory</param>
    /// <param name="configurator">An optional configurator for configuring the auto factory further</param>
    /// <returns></returns>
    public static IServiceCollection AddAutoFactory(
        this IServiceCollection services,
        Type factoryInterfaceType,
        Action<AutoFactoryConfiguration> configurator = null)
    {
        var configuration = new AutoFactoryConfiguration();
        configurator?.Invoke(configuration);

        services.Add(new(factoryInterfaceType, sp => CreateFactory(sp, factoryInterfaceType, configuration), configuration.Lifetime));
        return services;
    }

    private static object CreateFactory(IServiceProvider serviceProvider, Type factoryInterfaceType, AutoFactoryConfiguration autoFactoryConfiguration) => 
        ProxyGeneratorContainer.ProxyGeneratorInstance.CreateInterfaceProxyWithoutTarget(
            factoryInterfaceType,
            new FactoryInterceptor(serviceProvider, autoFactoryConfiguration)
        );

    private class FactoryInterceptor : IInterceptor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AutoFactoryConfiguration _autoFactoryConfiguration;
        private readonly ConcurrentDictionary<MethodInfo, ObjectFactory> _factories = new();

        internal FactoryInterceptor(IServiceProvider serviceProvider, AutoFactoryConfiguration autoFactoryConfiguration)
        {
            _serviceProvider = serviceProvider;
            _autoFactoryConfiguration = autoFactoryConfiguration;
        }

        public void Intercept(IInvocation invocation)
        {
            var factory = _factories.GetOrAdd(invocation.Method, CreateFactory);
            invocation.ReturnValue = factory(_serviceProvider, invocation.Arguments);
        }

        private ObjectFactory CreateFactory(MethodInfo method) => 
            ActivatorUtilities.CreateFactory(
                ResolveConcreteType(method.ReturnType),
                [.. method.GetParameters().Select(p => p.ParameterType)]);

        private Type ResolveConcreteType(Type returnType) => 
            _autoFactoryConfiguration.TypeMappings.TryGetValue(returnType, out var mapping) 
                ? mapping 
                : returnType;
    }    
}