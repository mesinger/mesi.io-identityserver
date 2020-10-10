using System.Linq;
using IdentityServer4.Models;
using Mesi.Io.IdentityServer4.Data.Entities;

namespace Mesi.Io.IdentityServer4.Config
{
    public static class MappingConfig
    {
        public static Client ToIdentityServerClient(this IdentityServerClient client)
        {
            if (client == null)
            {
                return null;
            }

            return new Client
            {
                Enabled = client.IsEnabled,
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                AllowedGrantTypes = client.AllowedGrantTypes.ToArray(),
                RequireClientSecret = client.RequireClientSecret,
                ClientSecrets = (from secret in client.ClientSecrets select new Secret(secret)).ToArray(),
                AccessTokenLifetime = client.AccessTokenLifetime,
                RedirectUris = client.RedirectUris.ToArray(),
                PostLogoutRedirectUris = client.PostLogoutRedirectUris.ToArray(),
                AllowedScopes = client.AllowedScopes.ToArray()
            };
        }
    }
}