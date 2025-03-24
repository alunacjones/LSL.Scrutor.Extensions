using System;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace LSL.Scrutor.Extensions;

internal class DelegatedRegistrationStrategy(Action<IServiceCollection, ServiceDescriptor> applicator, bool autoAdd) : RegistrationStrategy
{
    public override void Apply(IServiceCollection services, ServiceDescriptor descriptor)
    {
        if (autoAdd)
        {
            services.Add(descriptor);
        }

        applicator(services, descriptor);
    }
}