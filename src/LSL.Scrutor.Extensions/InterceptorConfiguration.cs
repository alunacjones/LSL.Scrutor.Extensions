using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace LSL.Scrutor.Extensions;

/// <summary>
/// Used to configure interceptors for a service
/// </summary>
public class InterceptorConfiguration
{
    private readonly List<Type> _interceptorTypes = [];

    /// <summary>
    /// Adds <c><typeparamref name="TInterceptor"/></c> to the list of interceptors to apply
    /// </summary>
    /// <typeparam name="TInterceptor"></typeparam>
    /// <returns></returns>
    public InterceptorConfiguration Add<TInterceptor>()
        where TInterceptor : IInterceptor
    {
        AddInterceptors(typeof(TInterceptor));
        return this;
    }

    /// <summary>
    /// Adds the <c><paramref name="types"/></c> to the list of interceptors to apply
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public InterceptorConfiguration AddInterceptors(IEnumerable<Type> types)
    {
        _interceptorTypes.AddRange(types);
        return this;
    }

    /// <summary>
    /// Adds the <c><paramref name="types"/></c> to the list of interceptors to apply
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public InterceptorConfiguration AddInterceptors(params Type[] types) => AddInterceptors(types.AsEnumerable());

    /// <summary>
    /// Implicity converts <c><see cref="InterceptorConfiguration"/></c> to an array of <c><see cref="Type"/></c>
    /// </summary>
    /// <param name="interceptor"></param>
    public static implicit operator Type[](InterceptorConfiguration interceptor) => [.. interceptor._interceptorTypes];
}

