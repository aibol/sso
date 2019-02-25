using System;
using System.Collections.Generic;
using IdentityServer.Models;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("test.api", "Test Api for Identity")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            // client credentials: test.cc
            var clientCredentials = new Client
            {
                ClientId = "test.cc",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = {"test.api"}
            };

            // resource owner password grant client: test.rop
            var resourceOwnerPassword = new Client
            {
                ClientId = "test.rop",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = {"test.api"}
            };

            var clients = new List<Client>
            {
                clientCredentials,
                resourceOwnerPassword
            };

            AddAibolInternalClients(clients);

            AddPingweiClients(clients);

            return clients;
        }

        private static void AddAibolInternalClients(List<Client> clients)
        {
            var ssoAdmin = new Client
            {
                ClientId = "aibol2.ssoadmin",
                ClientName = "Aibol2 System SSO Admin",
                AllowedGrantTypes = GrantTypes.Code,

                RequireConsent = false,

                ClientSecrets =
                {
                    new Secret("7fyrXe563_04".Sha256())
                },

                // where to redirect to after login
                RedirectUris =
                {
                    // development
                    "http://localhost:9000/signin-oidc",
                    // local host
                    "http://localhost:100/signin-oidc",
                    // test server host
                    "https://pass.aibol.com/admin/signin-oidc"
                },

                // where to redirect to after logout
                PostLogoutRedirectUris =
                {
                    // development
                    "http://localhost:9000/signout-callback-oidc",
                    // local host
                    "http://localhost:100/signout-callback-oidc",
                    // test server host
                    "https://pass.aibol.com/admin/signout-callback-oidc"
                },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "roles"
                }
            };

            clients.Add(ssoAdmin);
        }

        private static void AddPingweiClients(List<Client> clients)
        {
            var checkPoint = new Client
            {
                ClientId = "pingwei.pointcheck",
                ClientName = "平圩点检",
                AllowedGrantTypes = GrantTypes.Implicit,

                RequireConsent = false,

                ClientSecrets =
                {
                    new Secret("eiw8326Hw_53".Sha256())
                },

                // where to redirect to after login
                RedirectUris =
                {
                    // development
                    "https://localhost:44310/signin-oidc",
                    "http://localhost:25344/signin-oidc",
                    "http://localhost:25345/signin-oidc",
                    // local host
                    "http://localhost:82/signin-oidc",
                    // test server host
                    "http://dianjian.aibol.com.cn/signin-oidc"
                },

                // where to redirect to after logout
                PostLogoutRedirectUris =
                {
                    // development
                    "https://localhost:44310/signout-callback-oidc",
                    "http://localhost:25344/signout-callback-oidc",
                    "http://localhost:25345/signout-callback-oidc",
                    // local host
                    "http://localhost:82/signout-callback-oidc",
                    // test server host
                    "http://dianjian.aibol.com.cn/signout-callback-oidc"
                },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            };

            var backstage= new Client
            {
                ClientId = "pingwei.backstage",
                ClientName = "平圩工作区",
                AllowedGrantTypes = GrantTypes.Implicit,

                RequireConsent = false,

                ClientSecrets =
                {
                    new Secret("72psw8E25s_1Z".Sha256())
                },

                // where to redirect to after login
                RedirectUris =
                {
                    "http://localhost:81/",
                    "http://localhost:25341/",
                    "http://backstage.aibol.com.cn/"
                },

                // where to redirect to after logout
                PostLogoutRedirectUris =
                {
                    "http://localhost:81/",
                    "http://localhost:25341/",
                    "http://backstage.aibol.com.cn/"
                },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            };

            var equipment = new Client
            {
                ClientId = "pingwei.equipment",
                ClientName = "平圩工器具",
                AllowedGrantTypes = GrantTypes.Implicit,

                RequireConsent = false,

                ClientSecrets =
                {
                    new Secret("xDsrRrgJUQZ6_K1".Sha256())
                },

                // where to redirect to after login
                RedirectUris =
                {
                    "http://localhost:50829/",
                    "http://equipment.aibol.com.cn/"
                },

                // where to redirect to after logout
                PostLogoutRedirectUris =
                {
                    "http://localhost:50829/",
                    "http://equipment.aibol.com.cn/"
                },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            };

            clients.Add(checkPoint);
            clients.Add(backstage);
            clients.Add(equipment);
        }

        public static List<ApplicationUser> GetUsers()
        {
            // username: test@aibol.com 
            // password: Abc123!
            var user1 = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "test@aibol.com",
                NormalizedUserName = "TEST@AIBOL.COM",
                Email = "test@aibol.com",
                NormalizedEmail = "TEST@AIBOL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEEFkFEqj8bdWmOGsRGjunCEByf4o8Q8lEx1Ejr+eFd3nu+sr1TVhdzuQDA2k1FDy4w==",
                SecurityStamp = "EH3SZPZYPVTNLICM4Y3NESYUBKN6UHQJ",
                ConcurrencyStamp = "7a23aa1f-7081-4dfb-be0e-d6f4afd98abe",
                LockoutEnabled = true
            };

            return new List<ApplicationUser> {user1};
        }
    }
}