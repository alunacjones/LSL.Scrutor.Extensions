using System;
using System.Linq;
using Scrutor;

namespace LSL.Scrutor.Extensions;

internal static class ImplementationTypeFilterExtensions
{
    public static IImplementationTypeFilter CheckForMultipleLifetimes<TLifeTimeInterface>(this IImplementationTypeFilter source)
        where TLifeTimeInterface : ILifetimeDecoratedService
    {
        return source.Where(t =>
        {
            var count = t.GetInterfaces()
                .Where(i => i != typeof(ILifetimeDecoratedService))
                .Where(i => typeof(ILifetimeDecoratedService).IsAssignableFrom(i))
                .Count();

            if (count > 1)
            {
                throw new ArgumentException($"Type {t.FullName} implements many lifetime interfaces. You may only use one of IScopedService, ITransientService or ISingletonService");
            }

            return typeof(TLifeTimeInterface).IsAssignableFrom(t);
        });
    }
}