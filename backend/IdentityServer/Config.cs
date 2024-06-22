using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId()
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new(name: "product.read", displayName: "Read products")
    ];

    public static IEnumerable<Client> Clients =>
    [
        new()
        {
            ClientId = "petal-box-app",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            RequireClientSecret = false,
            AllowedScopes = { "product.read" }
        }
    ];
}