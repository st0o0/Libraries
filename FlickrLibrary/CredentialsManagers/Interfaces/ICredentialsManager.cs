using FlickrNet.Models;

namespace FlickrLibrary.CredentialsManagers.Interfaces
{
    public interface ICredentialsManager
    {
        bool SaveCredentials(string key, OAuthAccessToken token);

        bool TryGetCredentials(string key, out OAuthAccessToken token);

        OAuthAccessToken GetCredentials(string key);

        bool RemoveKey(string key);

        void Reload();

        void Clear();
    }
}