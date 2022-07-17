using FlickrLibrary.CredentialsManagers.Interfaces;
using FlickrNet.Common;
using FlickrNet.Models;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace FlickrLibrary.CredentialsManagers
{
    internal sealed class CredentialsManager : ICredentialsManager
    {
        private readonly ConcurrentDictionary<string, OAuthAccessToken> _keyValuePairs = new();

        private readonly string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FlickrNet", "file.dat");

        public CredentialsManager()
        {
            Reload();
        }

        public OAuthAccessToken GetCredentials(string key)
        {
            return _keyValuePairs[key];
        }

        public bool SaveCredentials(string key, OAuthAccessToken credential)
        {
            if (_keyValuePairs.ContainsKey(key))
            {
                return false;
            }
            _keyValuePairs.AddOrUpdate(key, credential, (s, item) => credential);
            Store();
            return true;
        }

        public bool RemoveKey(string key)
        {
            if (_keyValuePairs.ContainsKey(key))
            {
                return false;
            }
            _keyValuePairs.Remove(key, out _);
            Store();
            return true;
        }

        private void Reload()
        {
            lock (this)
            {
                using FileStream fs = new(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                using StreamReader sr = new(fs);
                _keyValuePairs.Clear();

                foreach (string item in sr.ReadToEnd().Split(Environment.NewLine))
                {
                    if (!(string.IsNullOrWhiteSpace(item) || string.IsNullOrEmpty(item)))
                    {
                        string[] values = AESHelper.Decrypt(item).Split(":");
                        _keyValuePairs[values[0]] = DecryptData(values[1]);
                    }
                }
            }
        }

        private void Store()
        {
            lock (this)
            {
                using FileStream fs = new(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                fs.Position = 0;
                using StreamWriter sw = new(fs);
                foreach (var item in _keyValuePairs)
                {
                    sw.WriteLine(AESHelper.Encrypt($"{item.Key}:{EncryptData(item.Value)}"));
                }
            }
        }

        private static OAuthAccessToken DecryptData(string value)
        {
            string[] items = AESHelper.Decrypt(value).Split("//");
            return new OAuthAccessToken()
            {
                Token = items[0],
                TokenSecret = items[1],
                FullName = items[2],
                UserId = items[3],
                Username = items[4]
            };
        }

        private static string EncryptData(OAuthAccessToken accessToken)
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