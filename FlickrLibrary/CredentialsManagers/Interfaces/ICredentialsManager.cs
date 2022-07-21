using FlickrNet.Models;

namespace FlickrLibrary.CredentialsManagers.Interfaces
{
    public interface ICredentialsManager
    {
        bool SaveCredentials(string key, OAuthAccessToken credential);

        OAuthAccessToken GetCredentials(string key);

        bool RemoveKey(string key);

        void Reload();

        void Clear();
    }
}