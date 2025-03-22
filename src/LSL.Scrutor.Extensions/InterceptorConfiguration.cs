using System;
using System.Collections.Generic;
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
        _interceptorTypes.Add(typeof(TInterceptor));
        return this;
    }

    /// <summary>
    /// Implicity converts <c><see cref="InterceptorConfiguration"/></c> to an array of <c><see cref="Type"/></c>
    /// </summary>
    /// <param name="interceptor"></param>
    public static implicit operator Type[](InterceptorConfiguration interceptor) => [.. interceptor._interceptorTypes];
}