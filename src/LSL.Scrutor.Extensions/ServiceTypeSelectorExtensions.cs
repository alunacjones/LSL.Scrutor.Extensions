using System;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace LSL.Scrutor.Extensions;

/// <summary>
/// ServiceTypeSelectorExtensions
/// </summary>
public static class ServiceTypeSelectorExtensions
{
    /// <summary>
    /// Registers a delegate as a <c><see cref="RegistrationStrategy"/></c>
    /// </summary>
    /// <remarks>
    /// If <c><paramref name="autoAdd"/></c> is <c>true</c> then the <c><see cref="ServiceDescriptor"/></c>
    /// will be added to the <c><see cref="IServiceCollection"/></c>
    /// prior to the <paramref name="applicator"/> being called
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="applicator">The delegate to use as the registration strategy</param>
    /// <param name="autoAdd">
    /// If <c>true</c> then the <c><see cref="ServiceDescriptor"/></c>
    /// will be added to the <c><see cref="IServiceCollection"/></c>
    /// prior to the <paramref name="applicator"/> being called
    /// </param>
    /// <returns></returns>
    public static IServiceTypeSelector WithRegistrationStrategy(
        this IServiceTypeSelector source,
        Action<IServiceCollection, ServiceDescriptor> applicator,
        bool autoAdd = true) => 
        source.UsingRegistrationStrategy(new DelegatedRegistrationStrategy(applicator, autoAdd));
}
