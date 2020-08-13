using IdentityServer4.Stores;
using Mesi.Io.IdentityServer4.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Mesi.Io.IdentityServer4.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection service)
        {
            return service
                .AddIdentityServerDependencies()
                .AddOwnDependencies();
        }

        private static IServiceCollection AddOwnDependencies(this IServiceCollection service)
        {
            service.AddScoped<IRegistrationService, IdentityRegistrationService>();

            return service;
        }
        
        private static IServiceCollection AddIdentityServerDependencies(this IServiceCollection service)
        {
            service.AddScoped<ISigningCredentialStore, SigningCredentialStore>();
            service.AddScoped<IValidationKeysStore, ValidationKeyStore>();

            return service;
        }
    }
}