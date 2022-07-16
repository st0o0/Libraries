using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlickrLibrary.CredentialsManagers.Interfaces
{
    public interface ICredential
    {
        string CredentialName { get; init; }

        string Password { get; init; }
    }
}