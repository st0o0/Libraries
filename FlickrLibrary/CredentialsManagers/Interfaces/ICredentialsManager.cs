using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlickrLibrary.CredentialsManagers.Interfaces
{
    public interface ICredentialsManager
    {
        void SaveCredentials(ICredential credential);

        ICredential GetCredentials();
    }
}