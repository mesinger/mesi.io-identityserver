using System.Collections.Generic;

namespace Mesi.Io.IdentityServer4.Data.Entities
{
    public class IdentityServerClient
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public bool IsEnabled { get; set; }
        public IEnumerable<string> AllowedGrantTypes { get; set; }
        public bool RequireClientSecret { get; set; }
        public IEnumerable<string> ClientSecrets { get; set; }
        public int AccessTokenLifetime { get; set; }
        public IEnumerable<string> RedirectUris { get; set; }
        public IEnumerable<string> PostLogoutRedirectUris { get; set; }
        public IEnumerable<string> AllowedScopes { get; set; }
    }
}