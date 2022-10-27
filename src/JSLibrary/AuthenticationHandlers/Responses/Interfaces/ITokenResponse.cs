namespace JSLibrary.AuthenticationHandlers.Responses.Interfaces
{
    public interface ITokenResponse
    {
        string AccessToken { get; set; }

        long ExpirationInSeconds { get; set; }

        string Scheme { get; set; }
    }
}
