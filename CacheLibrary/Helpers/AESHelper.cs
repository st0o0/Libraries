using System.Security.Cryptography;
using System.Text;

namespace CacheLibrary.Helpers
{
    public static class AESHelper
    {
        private static byte[] _aesIV128 = Encoding.UTF8.GetBytes(StringGenerator.GenerateString(16));

        private static byte[] _aesKey256 = Encoding.UTF8.GetBytes(StringGenerator.GenerateString(32));

        public static string AESKey => Encoding.UTF8.GetString(_aesKey256);

        public static string AESIV => Encoding.UTF8.GetString(_aesIV128);

        public static void SetKey(string value) => _aesKey256 = Encoding.UTF8.GetBytes(value);

        public static void SetIV(string value) => _aesIV128 = Encoding.UTF8.GetBytes(value);

        public static string Encrypt(string value)
        {
            using Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.IV = _aesIV128;
            aes.Key = _aesKey256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using StreamWriter sw = new(cs);
            sw.Write(value);
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static string Decrypt(string value)
        {
            using Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.IV = _aesIV128;
            aes.Key = _aesKey256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using MemoryStream ms = new(Encoding.UTF8.GetBytes(value));
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            using StreamReader sr = new(cs);
            return sr.ReadToEnd();
        }

        public static class StringGenerator
        {
            public static string GenerateString(int length)
            {
                return GenerateString(new Random(DateTime.Now.Millisecond), length);
            }

            public static string GenerateString(Random random, int length)
            {
                random ??= new(DateTime.Now.Millisecond);

                StringBuilder sb = new();
                for (int i = 0; i <= length; i++)
                {
                    sb.Append(GenerateChar(random));
                }
                return sb.ToString();
            }

            private static char GenerateChar(Random random)
            {
                int asciiCharacterStart = 65;
                int asciiCharacterEnd = 122;
                return (char)(random.Next(asciiCharacterStart, asciiCharacterEnd + 1) % 255);
            }
        }
    }

    public sealed class AESPipeline : IDisposable
    {
        private readonly Aes aes = Aes.Create();

        public AESPipeline()
        {
        }

        public AESPipeline SetKey(string value, int keySize)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            aes.Key = Encoding.UTF8.GetBytes(value);
            aes.KeySize = keySize;
            return this;
        }

        public AESPipeline SetIV(string value, int blockSize)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            aes.IV = Encoding.UTF8.GetBytes(value);
            aes.BlockSize = blockSize;
            return this;
        }

        public string Encrypt(string value)
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using StreamWriter sw = new(cs);
            sw.Write(value);
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public string Decrypt(string value)
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using MemoryStream ms = new(Encoding.UTF8.GetBytes(value));
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            using StreamReader sr = new(cs);
            return sr.ReadToEnd();
        }

        public void Dispose()
        {
            aes.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}