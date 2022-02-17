namespace JSLibrary.AuthenticationHandlers.Credentials.Interfaces
{
    public interface IClientCredentials
    {
        string ClientId { get; set; }

        string ClientSecret { get; set; }
    }
}