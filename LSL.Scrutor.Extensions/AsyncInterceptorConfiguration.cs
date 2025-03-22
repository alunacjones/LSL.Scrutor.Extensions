using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace LSL.Scrutor.Extensions;

/// <summary>
/// Used to configure async interceptors for a service
/// </summary>
public class AsyncInterceptorConfiguration
{
    private readonly List<Type> _interceptorTypes = [];

    /// <summary>
    /// Adds <c><typeparamref name="TInterceptor"/></c> to the list of interceptors to apply
    /// </summary>
    /// <typeparam name="TInterceptor"></typeparam>
    /// <returns></returns>
    public AsyncInterceptorConfiguration Add<TInterceptor>()
        where TInterceptor : IAsyncInterceptor
    {
        _interceptorTypes.Add(typeof(TInterceptor));
        return this;
    }

    /// <summary>
    /// Implicity converts <c><see cref="InterceptorConfiguration"/></c> to an array of <c><see cref="Type"/></c>
    /// </summary>
    /// <param name="interceptor"></param>
    public static implicit operator Type[](AsyncInterceptorConfiguration interceptor) => [.. interceptor._interceptorTypes];
}