using FlickrLibrary.CredentialsManagers.Interfaces;
using FlickrNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FlickrLibrary.CredentialsManagers
{
    internal sealed class CredentialsManager : ICredentialsManager
    {
        private readonly byte[] salt = Encoding.UTF8.GetBytes("vjEyLnw6ZKIECNFkm0tB");

        public CredentialsManager()
        {
        }

        private OAuthAccessToken DecodingData(byte[] bytes)
        {
            string[] items = Encoding.UTF8.GetString(bytes).Split("//", StringSplitOptions.TrimEntries);
            return new OAuthAccessToken()
            {
                Token = items[0],
                TokenSecret = items[1],
                FullName = items[2],
                UserId = items[3],
                Username = items[4]
            };
        }

        private string EncodingData(OAuthAccessToken accessToken)
        {
            StringBuilder sb = new StringBuilder().AppendJoin("//", new[] { accessToken.Token, accessToken.TokenSecret, accessToken.FullName, accessToken.UserId, accessToken.Username });
            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return Convert.ToBase64String(bytes);
        }
    }
}