using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace eShop.Identity;

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
            new ApiScope("catalogapi"),
            new ApiScope("basketapi"),
            new ApiScope("catalogapi.read"),
            new ApiScope("catalogapi.write"),
            new ApiScope("eshoppinggateway")
        };

    public static IEnumerable<ApiResource> ApiResource =>
      new ApiResource[]
      {
        new ApiResource("Catalog", "Catalog.API")
        {
            Scopes={ "catalogapi.read", "catalogapi.write" }
        },
        new ApiResource("Basket", "Basket.API")
        {
            Scopes={"basketapi"}
        },
        new ApiResource("EShoppingGateway", "EShopping Gateway")
        {
            Scopes={"eshoppinggateway","basketapi","catalogapi.read","catalogapi.write"}
        }
      };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            
              new Client
              {
                ClientName = "Catalog API Client",
                ClientId = "CatalogApiClient",
                ClientSecrets = { new Secret("49C1A7A9-1C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "catalogapi.read", "catalogapi.write" }
              },
              new Client
              {
                ClientName = "Basket API Client",
                ClientId = "BasketApiClient",
                ClientSecrets = { new Secret("49C1A7B8-1C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"basketapi"}
              },
              new Client
              {
                ClientName = "EShopping Gateway Client",
                ClientId = "EShoppingGatewayClient",
                ClientSecrets = { new Secret("49C1A7B8-1C76-4A89-A3C6-A37998FB86B0".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "eshoppinggateway", "basketapi", "catalogapi.read", "catalogapi.write" }
              },
              new Client
                {
                    ClientId = "angular-client",
                    ClientName = "Angular SPA",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,            // SPA
                    RedirectUris = {
                        "http://localhost:4200/signin-callback",
                        "http://localhost:4200/assets/silent-callback.html",
                        "https://ecommerce-microservice-fawn.vercel.app/signin-callback",
                        "https://ecommerce-microservice-fawn.vercel.app/assets/silent-callback.html",
                        "https://client-seven-lac-37.vercel.app/signin-callback",
                        "https://client-seven-lac-37.vercel.app/assets/silent-callback.html"
                    },
                    PostLogoutRedirectUris = {
                        "http://localhost:4200/signout-callback",
                        "https://ecommerce-microservice-fawn.vercel.app/signout-callback",
                        "https://client-seven-lac-37.vercel.app/signout-callback"
                    },
                    AllowedCorsOrigins = {
                        "http://localhost:4200",
                        "https://ecommerce-microservice-fawn.vercel.app",
                        "https://client-seven-lac-37.vercel.app"
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "eshoppinggateway"
                    },
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600,
                    Enabled = true
                }
        };
}
