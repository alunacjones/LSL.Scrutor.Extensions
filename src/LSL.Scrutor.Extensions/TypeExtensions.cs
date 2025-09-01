using System;
using System.Linq;

namespace LSL.Scrutor.Extensions;

internal static class TypeExtensions
{
    public static Type EnsureClosedGenericType(this Type source, object closedInstance)
    {
        if (source.IsGenericType is not true) return source;

        return closedInstance
            .GetType()
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == source);
    }
}