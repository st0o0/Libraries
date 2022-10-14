namespace JSLibrary.AuthenticationHandlers.Responses.Interfaces
{
    public interface ITokenResponse
    {
        string AccessToken { get; set; }

        string Scheme { get; set; }

        long ExpirationInSeconds { get; set; }
    }
}