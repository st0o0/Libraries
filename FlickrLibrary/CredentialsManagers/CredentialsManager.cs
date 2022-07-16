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
            string[] items = AESHelper.Decrypt(Encoding.UTF8.GetString(bytes)).Split("//");
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
            return AESHelper.Encrypt(sb.ToString());
        }

        private sealed class AESHelper
        {
            private static readonly byte[] _aesIV256 = Encoding.UTF8.GetBytes(@"!QAZ2WSX#EDC4RFV");

            private static readonly byte[] _aesKey256 = Encoding.UTF8.GetBytes(@"5TGB&YHN7UJM(IK<5TGB&YHN7UJM(IK<");

            public static string Encrypt(string plainText)
            {
                using Aes aes = Aes.Create();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.IV = _aesIV256;
                aes.Key = _aesKey256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using MemoryStream ms = new();
                using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
                using StreamWriter sw = new(cs);
                sw.Write(plainText);
                return Encoding.UTF8.GetString(ms.ToArray());
            }

            public static string Decrypt(string text)
            {
                using Aes aes = Aes.Create();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.IV = _aesIV256;
                aes.Key = _aesKey256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using MemoryStream ms = new(Encoding.UTF8.GetBytes(text));
                using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
                using StreamReader sr = new(cs);
                return sr.ReadToEnd();
            }
        }
    }
}