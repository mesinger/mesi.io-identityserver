// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(), 
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("test.read", "IdentityServer4 Test API"), 
                new ApiScope("test.write", "IdentityServer4 Test API"), 
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("test", "Test API")
                {
                    Scopes = {"test.read", "test.write"},
                    UserClaims = {"name", "email"}
                },
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"test.read", "test.write"}
                }, 
                new Client()
                {
                    ClientId = "mvc",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"https://localhost:7001/signin-oidc"},
                    PostLogoutRedirectUris = {"https://localhost:7001/signout-callback-oidc"},
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "test.read",
                        "test.write"
                    },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                }, 
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris =           { "http://localhost:8080/login-callback" },
                    PostLogoutRedirectUris = { "http://localhost:8080" },
                    AllowedCorsOrigins =     { "http://localhost:8080" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "test"
                    }
                }
            };
    }
}