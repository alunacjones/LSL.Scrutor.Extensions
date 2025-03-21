using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Castle.DynamicProxy;
using static LSL.Scrutor.Extensions.ProxyGeneratorContainer;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// ProxiedDecorators
/// </summary>
public static class InterceptedDecorators
{   
    /// <summary>
    /// Decorate a service with a <c>Castle.Core</c> <c><see cref="IInterceptor"/></c>
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TToDecorate"></typeparam>
    /// <typeparam name="TInterceptor"></typeparam>
    /// <returns></returns>
    public static IServiceCollection DecorateWithInterceptor<TToDecorate, TInterceptor>(this IServiceCollection source)
        where TInterceptor : IInterceptor
        where TToDecorate : class
    {
        source.Decorate(
            typeof(TToDecorate), 
            (service, serviceProvider) => 
            { 
                var interceptor = serviceProvider.GetRequiredService<TInterceptor>();
                return ProxyGeneratorInstance.CreateInterfaceProxyWithTarget(typeof(TToDecorate), service, interceptor);
            });

        return source;
    }

    public static IServiceCollection DecorateWithAsyncInterceptor<TToDecorate, TAsyncInterceptor>(this IServiceCollection source)
        where TAsyncInterceptor : IAsyncInterceptor
        where TToDecorate : class
    {
        source.Decorate(
            typeof(TToDecorate), 
            (service, serviceProvider) => 
            { 
                var interceptor = serviceProvider.GetRequiredService<TAsyncInterceptor>();
                return ProxyGeneratorInstance.CreateInterfaceProxyWithTarget(typeof(TToDecorate), service, interceptor);
            });

        return source;
    }

    /// <summary>
    /// Adds all <c><see cref="IInterceptor"/></c> instances registered in the given assemblies
    /// </summary>
    /// <param name="source"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddInterceptorsFromAssemblies(this IServiceCollection source, IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            assembly.GetTypes()
                .Where(t => !t.IsAbstract && t.IsClass && (typeof(IInterceptor).IsAssignableFrom(t) || typeof(IAsyncInterceptor).IsAssignableFrom(t)))
                .ToList()
                .ForEach(t => source.AddScoped(t));
        }

        return source;
    }

    /// <summary>
    /// Adds all <c><see cref="IInterceptor"/></c> instances registered in the given assemblies
    /// </summary>
    /// <param name="source"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddInterceptorsFromAssemblies(this IServiceCollection source, params Assembly[] assemblies) => 
        source.AddInterceptorsFromAssemblies(assemblies.AsEnumerable());

    /// <summary>
    /// Adds all <c><see cref="IInterceptor"/></c> instances registered in the assembly that contains <typeparamref name="T"/>
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddInterceptorsFromAssemblyOf<T>(this IServiceCollection source) => 
        source.AddInterceptorsFromAssemblies(typeof(T).Assembly);
}