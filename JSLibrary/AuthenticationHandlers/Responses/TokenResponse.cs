using JSLibrary.AuthenticationHandlers.Responses.Interfaces;

namespace JSLibrary.AuthenticationHandlers.Responses
{
    public class TokenResponse : ITokenResponse
    {
        public string AccessToken { get; set; }
        public string Scheme { get; set; }
        public long ExpirationInSeconds { get; set; }
    }
}