using Castle.DynamicProxy;

namespace LSL.Scrutor.Extensions;

/// <summary>
/// A container for the <c>Castle.Core</c> <c><see cref="ProxyGenerator"/></c>
/// </summary>
public static class ProxyGeneratorContainer
{
    private static readonly ProxyGenerator _proxyGenerator = new();

    /// <summary>
    /// Provides the global <c><see cref="ProxyGenerator"/></c> instance
    /// </summary>
    public static ProxyGenerator ProxyGeneratorInstance => _proxyGenerator;
}