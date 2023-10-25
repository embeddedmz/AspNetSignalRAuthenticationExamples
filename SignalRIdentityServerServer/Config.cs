using IdentityServer4.Models;

namespace SignalRIdentityServerServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> Apis
        {
            get
            {
                return new List<ApiResource>()
                {
                    new ApiResource("prevo100-api", "Prevo100 API")
                };
            }
        }

        public static IEnumerable<Client> Clients
        {
            get
            {
                return new List<Client>()
                {
                    new Client
                    {
                        ClientId = "client",
                        AllowedScopes = { "prevo100-api" },
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets = {
                            new Secret("Prevo100".Sha256())
                        }
                    }
                };
            }
        }

        public static IEnumerable<ApiScope> Scopes
        {
            get
            {
                return new List<ApiScope>()
                {
                    new ApiScope("prevo100-api")
                };
            }
        }
    }
}
