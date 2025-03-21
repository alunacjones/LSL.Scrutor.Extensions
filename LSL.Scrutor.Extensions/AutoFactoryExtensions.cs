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
    /// Automatically adds a factory for the given interface
    /// </summary>
    /// <param name="services"></param>
    /// <param name="concreteType"></param>
    /// <typeparam name="TFactory"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddAutoFactory<TFactory>(this IServiceCollection services, Type concreteType = null)
        where TFactory : class
    {
        services.AddSingleton(sp => CreateFactory<TFactory>(sp, concreteType));
        return services;
    }

    private static TFactory CreateFactory<TFactory>(IServiceProvider serviceProvider, Type concreteType = null)
        where TFactory : class
    {
        return ProxyGeneratorContainer.ProxyGeneratorInstance.CreateInterfaceProxyWithoutTarget<TFactory>(
            new FactoryInterceptor(serviceProvider, concreteType));
    }

    private class FactoryInterceptor : IInterceptor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Type _concreteType;
        private readonly ConcurrentDictionary<MethodInfo, ObjectFactory> _factories;

        public FactoryInterceptor(IServiceProvider serviceProvider, Type concreteType)
        {
            _serviceProvider = serviceProvider;
            _concreteType = concreteType;
            _factories = new ConcurrentDictionary<MethodInfo, ObjectFactory>();
        }

        public void Intercept(IInvocation invocation)
        {
            var factory = _factories.GetOrAdd(invocation.Method, CreateFactory);
            invocation.ReturnValue = factory(_serviceProvider, invocation.Arguments);
        }

        private ObjectFactory CreateFactory(MethodInfo method)
        {
            return ActivatorUtilities.CreateFactory(
                _concreteType ?? method.ReturnType,
                [.. method.GetParameters().Select(p => p.ParameterType)]);
        }
    }    
}