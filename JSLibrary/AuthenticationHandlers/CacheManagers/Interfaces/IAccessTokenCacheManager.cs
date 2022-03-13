using JSLibrary.AuthenticationHandlers.Responses.Interfaces;

namespace JSLibrary.AuthenticationHandlers.CacheManagers.Interfaces
{
    public interface IAccessTokenCacheManager
    {
        void AddOrUpdateToken(string clientId, ITokenResponse accessToken);

        void Clear();

        ITokenResponse GetToken(string clientId);
    }
}