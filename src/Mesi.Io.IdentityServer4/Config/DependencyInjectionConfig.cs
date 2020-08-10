using Mesi.Io.IdentityServer4.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Mesi.Io.IdentityServer4.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection service)
        {
            service.AddTransient<IRegistrationService, IdentityRegistrationService>();

            return service;
        }
    }
}