using FlickrNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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