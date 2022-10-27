using JSLibrary.AuthenticationHandlers.Credentials.Interfaces;

namespace JSLibrary.AuthenticationHandlers.Credentials
{
    public class ClientCredentials : IClientCredentials
    {
        public string ClientId { get; set; } = null;

        public string ClientSecret { get; set; } = null;
    }
}
