using JSLibrary.AuthenticationHandlers.Responses.Interfaces;

namespace JSLibrary.AuthenticationHandlers.CacheManagers.Interfaces
{
    public interface IAccessTokenCacheManager
    {
        void AddOrUpdateToken(string clientId, ITokenResponse accessToken);

        ITokenResponse GetToken(string clientId);
    }
}