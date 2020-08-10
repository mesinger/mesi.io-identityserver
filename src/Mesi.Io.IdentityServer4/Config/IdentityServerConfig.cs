// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Mesi.Io.IdentityServer4.Config
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(), 
                new IdentityResources.Email(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("clipboard", "Clipboard API")
                {
                    Scopes = {"clipboard.user.read", "clipboard.user.write"},
                    UserClaims = {"name", "email"}
                },
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new []
            {
                new ApiScope("clipboard.user.read", "Clipboard API User Read Access"), 
                new ApiScope("clipboard.user.write", "Clipboard API User Write Access"),
            };

        public static IEnumerable<Client> Clients =>
            new []
            {
                new Client
                {
                    ClientId = "vuejs-client",
                    ClientName = "VueJS Web Frontend",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    
                    RedirectUris = { "http://localhost:8080/auth/login-callback" },
                    PostLogoutRedirectUris = { "http://localhost:8080" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "clipboard.user.read",
                        "clipboard.user.write",
                    }
                }, 
            };
    }
}