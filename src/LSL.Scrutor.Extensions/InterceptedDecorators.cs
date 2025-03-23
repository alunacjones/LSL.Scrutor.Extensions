using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using LSL.Scrutor.Extensions;
using static LSL.Scrutor.Extensions.ProxyGeneratorContainer;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// InterceptedDecoratorsExtensions
/// </summary>
public static class InterceptedDecoratorsExtensions
{
    /// <summary>
    /// Decorates <c><paramref name="serviceTypeToDecorate"/></c> with the given <c><paramref name="interceptorTypes"/></c>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serviceTypeToDecorate"></param>
    /// <param name="interceptorTypes"></param>
    /// <returns></returns>    
    public static IServiceCollection DecorateWithInterceptors(
        this IServiceCollection services,
        Type serviceTypeToDecorate,
        IEnumerable<Type> interceptorTypes) 
    {
        if (!interceptorTypes.Any())
        {
            throw new ArgumentException("No interceptors have been provided", nameof(interceptorTypes));
        }

        foreach (var interceptorType in interceptorTypes)
        {
            services.Decorate(serviceTypeToDecorate, (service, serviceProvider) =>
            {
                if (typeof(IInterceptor).IsAssignableFrom(interceptorType))
                {
                    return ProxyGeneratorInstance.CreateInterfaceProxyWithTarget(
                        serviceTypeToDecorate,
                        service,
                        (IInterceptor)serviceProvider.GetRequiredService(interceptorType));
                }

                if (typeof(IAsyncInterceptor).IsAssignableFrom(interceptorType))
                {
                    return ProxyGeneratorInstance.CreateInterfaceProxyWithTarget(
                        serviceTypeToDecorate,
                        service,
                        (IAsyncInterceptor)serviceProvider.GetRequiredService(interceptorType));
                }

                throw new ArgumentException(
                    "The interceptor type must be either an IInterceptor or an IAsyncInterceptor",
                    nameof(interceptorType));
            });
        }

        return services;
    }

    /// <summary>
    /// Decorates <c><paramref name="serviceTypeToDecorate"/></c> with the given <c><paramref name="interceptorTypes"/></c>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serviceTypeToDecorate"></param>
    /// <param name="interceptorTypes"></param>
    /// <returns></returns>
    public static IServiceCollection DecorateWithInterceptors(this IServiceCollection services, Type serviceTypeToDecorate, params Type[] interceptorTypes) => 
        services.DecorateWithInterceptors(serviceTypeToDecorate, interceptorTypes.AsEnumerable());

    /// <summary>
    /// Decorate <c><typeparamref name="TToDecorate"/></c> with <c><typeparamref name="TInterceptor"/></c>
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TToDecorate"></typeparam>
    /// <typeparam name="TInterceptor"></typeparam>
    /// <returns></returns>
    public static IServiceCollection DecorateWithInterceptor<TToDecorate, TInterceptor>(this IServiceCollection source)
        where TInterceptor : IInterceptor
        where TToDecorate : class =>
        source.DecorateWithInterceptors(typeof(TToDecorate), typeof(TInterceptor));

    /// <summary>
    /// Decorate <c><typeparamref name="TToDecorate"/></c> with the provided <c><paramref name="interceptorTypes"/></c>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="interceptorTypes"></param>
    /// <typeparam name="TToDecorate"></typeparam>
    /// <returns></returns>
    public static IServiceCollection DecorateWithInterceptors<TToDecorate>(this IServiceCollection source, IEnumerable<Type> interceptorTypes)
        where TToDecorate : class =>
        source.DecorateWithInterceptors(typeof(TToDecorate), interceptorTypes);

    /// <summary>
    /// Decorate <c><typeparamref name="TToDecorate"/></c> with the provided <c><paramref name="interceptorTypes"/></c>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="interceptorTypes"></param>
    /// <typeparam name="TToDecorate"></typeparam>
    /// <returns></returns>
    public static IServiceCollection DecorateWithInterceptors<TToDecorate>(this IServiceCollection source, params Type[] interceptorTypes)
        where TToDecorate : class =>
        source.DecorateWithInterceptors(typeof(TToDecorate), interceptorTypes);

    /// <summary>
    /// Decorate <c><typeparamref name="TToDecorate"/></c> using the provided <paramref name="configurator"/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <typeparam name="TToDecorate"></typeparam>
    /// <returns></returns>
    public static IServiceCollection DecorateWithInterceptors<TToDecorate>(this IServiceCollection source, Action<InterceptorConfiguration> configurator)
        where TToDecorate : class
    {
        var configuration = new InterceptorConfiguration();
        configurator?.Invoke(configuration);

        return source.DecorateWithInterceptors<TToDecorate>(configuration);
    }

    /// <summary>
    /// Decorate <c><typeparamref name="TToDecorate"/></c> using the provided <paramref name="configurator"/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <typeparam name="TToDecorate"></typeparam>
    /// <returns></returns>
    public static IServiceCollection DecorateWithAsyncInterceptors<TToDecorate>(this IServiceCollection source, Action<AsyncInterceptorConfiguration> configurator)
        where TToDecorate : class
    {
        var configuration = new AsyncInterceptorConfiguration();
        configurator?.Invoke(configuration);

        return source.DecorateWithInterceptors<TToDecorate>(configuration);
    }

    /// <summary>
    /// Decorate <c><typeparamref name="TToDecorate"/></c> with the <c><typeparamref name="TAsyncInterceptor"/></c>
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TToDecorate"></typeparam>
    /// <typeparam name="TAsyncInterceptor"></typeparam>
    /// <returns></returns>
    public static IServiceCollection DecorateWithAsyncInterceptor<TToDecorate, TAsyncInterceptor>(this IServiceCollection source)
        where TAsyncInterceptor : IAsyncInterceptor
        where TToDecorate : class =>
        source.DecorateWithInterceptors(typeof(TToDecorate), typeof(TAsyncInterceptor));

    /// <summary>
    /// Adds all <c><see cref="IInterceptor"/></c> and all <c><see cref="IAsyncInterceptor"/></c> types in the given assemblies
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
    /// Adds all <c><see cref="IInterceptor"/></c> and all <c><see cref="IAsyncInterceptor"/></c> types in the given assemblies
    /// </summary>
    /// <param name="source"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddInterceptorsFromAssemblies(this IServiceCollection source, params Assembly[] assemblies) =>
        source.AddInterceptorsFromAssemblies(assemblies.AsEnumerable());

    /// <summary>
    /// Adds all <c><see cref="IInterceptor"/></c> and all <c><see cref="IAsyncInterceptor"/></c> types 
    /// in the assembly that contains <typeparamref name="T"/>
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddInterceptorsFromAssemblyOf<T>(this IServiceCollection source) =>
        source.AddInterceptorsFromAssemblies(typeof(T).Assembly);
}