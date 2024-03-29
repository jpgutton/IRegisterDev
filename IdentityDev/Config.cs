﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityDev
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("", "BackEnd API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                //new Client
                //{
                //    ClientId = "client",
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,

                //    ClientSecrets = 
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = { "IRegisterBackEndAPI" }
                //},

                // resource owner password grant client
                //new Client
                //{
                //    ClientId = "ro.client",
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                //    ClientSecrets = 
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = { "api1" }
                //},

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "DashBoard",
                    ClientName = "DashBoard Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("secret-".Sha256())
                    },

                    RedirectUris = { "http://localhost:45102/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:45102" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        ""
                    },
                    AllowOfflineAccess = true
                }            };
        }
    }
}