using System;
using System.Collections.Generic;
using System.Linq;

namespace LSL.Scrutor.Extensions;

internal static class GuardExtensions
{
    public static T GuardAgainstNull<T>(this T source, string parameterName)
    {
        if (source == null) throw new ArgumentNullException(parameterName);

        return source;
    }

    public static IEnumerable<T> GuardAgainstEmpty<T>(this IEnumerable<T> source, string parameterName)
    {
        if (!source.Any()) throw new ArgumentException("Enumerable must contain at least one value", parameterName);

        return source;
    }
}