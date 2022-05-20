using Open.OAuth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Open.OAuth
{
    public class OAuthClient
    {
        private readonly Random nonceGenerator;

        protected OAuthClient()
        {
            this.nonceGenerator = new();
        }

        public async Task<OAuthToken> GetRequestTokenAsync(string requestTokenUri, string oauthConsumerKey, string oauthConsumerSecret, string callbackUrl, Dictionary<string, string> parameters = null, CancellationToken cancellationToken = default)
        {
            var url = CreateOAuthUrl(requestTokenUri, oauthConsumerKey, oauthConsumerSecret, oauthCallback: callbackUrl, parameters: parameters);
            HttpClient client = CreateHttpClient();
            var response = await client.GetAsync(url, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                Dictionary<string, string> r = ParseResponse(await response.Content.ReadAsStringAsync(cancellationToken));
                var oauthCallbackConfirmed = true;
                if (r.ContainsKey("oauth_callback_confirmed"))
                {
                    if (!bool.TryParse(r["oauth_callback_confirmed"], out oauthCallbackConfirmed))
                    {
                        if (int.TryParse(r["oauth_callback_confirmed"], out int intBoolean))
                        {
                            oauthCallbackConfirmed = intBoolean == 1;
                        }
                    }
                }
                return new OAuthToken()
                {
                    Token = r["oauth_token"],
                    TokenSecret = r["oauth_token_secret"],
                    CallbackConfirmed = oauthCallbackConfirmed,
                };
            }
            else
            {
                response.EnsureSuccessStatusCode();
                return null;
            }
        }

        public string GetAuthorizeUrl(string authorizeUri, string oauthConsumerKey, string oauthConsumerSecret, string requestToken)
        {
            return CreateOAuthUrl(authorizeUri, oauthConsumerKey, oauthConsumerSecret, oauthToken: requestToken);
        }

        public string GetAuthorizeUrl(string authorizeUri, string oauthConsumerKey, string oauthConsumerSecret, string requestToken, string redirectUrl)
        {
            return CreateOAuthUrl(authorizeUri, oauthConsumerKey, oauthConsumerSecret, oauthToken: requestToken, oauthCallback: redirectUrl);
        }

        public async Task<OAuthToken> GetAccessTokenAsync(
            string accessTokenUri,
            string oauthConsumerKey,
            string oauthConsumerSecret,
            string oauthToken,
            string oauthTokenSecret,
            string oauthVerifier,
            CancellationToken cancellationToken = default)
        {
            var url = CreateOAuthUrl(accessTokenUri, oauthConsumerKey, oauthConsumerSecret, oauthToken: oauthToken, oauthTokenSecret: oauthTokenSecret, oauthVerifier: oauthVerifier, oauthCallback: null);
            var client = CreateHttpClient();
            var response = await client.GetAsync(url, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                Dictionary<string, string> r = ParseResponse(await response.Content.ReadAsStringAsync(cancellationToken));
                return new OAuthToken()
                {
                    Token = r["oauth_token"],
                    TokenSecret = r["oauth_token_secret"]
                };
            }
            else
            {
                response.EnsureSuccessStatusCode();
                return null;
            }
        }

        protected Dictionary<string, string> ParseResponse(string responseString)
        {
            Dictionary<string, string> parameters = new();
            foreach (var str in responseString.Split('&'))
            {
                var parts = str.Split('=');
                if (parts.Length == 2)
                {
                    parameters[parts[0]] = parts[1];
                }
            }
            return parameters;
        }

        /// <summary>
        /// Generates a new nonce, must be unique for each OAuth request
        /// </summary>
        /// <returns>nonce as string</returns>
        public string Nonce()
        {
            return nonceGenerator.Next(123400, 9999999).ToString();
        }

        /// <summary>
        /// Gets a timestamp in the format required for OAuth
        /// </summary>
        /// <returns>timestamp as string</returns>
        public string Timestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// Generates a URL with OAuth parameters
        /// </summary>
        /// <param name="url">base URL for request</param>
        /// <param name="mode">GET or POST, must match HTTP method used</param>
        /// <returns>OAuth compatible URL</returns>
        public string CreateOAuthUrl(
            string url,
            string oauthConsumerKey,
            string oauthConsumerSecret,
            string oauthToken = null,
            string oauthTokenSecret = null,
            string oauthCallback = null/*"oob"*/,
            string oauthVerifier = null,
            Methode mode = Methode.GET,
            IComparer<string> comparer = null,
            Dictionary<string, string> parameters = null)
        {
            var parametersList = parameters == null ? new List<string>() : parameters.Select(pair => string.Format("{0}={1}", pair.Key, Uri.EscapeDataString(pair.Value ?? ""))).ToList();
            parametersList.Add("oauth_consumer_key=" + oauthConsumerKey);
            parametersList.Add("oauth_nonce=" + Nonce());
            parametersList.Add("oauth_signature_method=HMAC-SHA1");
            parametersList.Add("oauth_version=1.0");
            if (oauthCallback != null)
            {
                parametersList.Add("oauth_callback=" + Uri.EscapeDataString(oauthCallback));
            }

            parametersList.Add("oauth_timestamp=" + Timestamp());
            if (oauthToken != null)
            {
                parametersList.Add("oauth_token=" + oauthToken);
            }

            if (oauthVerifier != null)
            {
                parametersList.Add("oauth_verifier=" + oauthVerifier);
            }

            if (comparer != null)
            {
                parametersList = parametersList.OrderBy(s => s[..s.IndexOf("=")], comparer).ToList();
            }
            else
            {
                parametersList = parametersList.OrderBy(s => s[..s.IndexOf("=")]).ToList();
            }

            string parametersStr = string.Join("&", parametersList.ToArray());

            string baseStr = mode.ToString() + "&" + Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(parametersStr);

            parametersList.Add("oauth_signature=" + ComputeHashString(oauthConsumerSecret, oauthTokenSecret, baseStr));
            parametersList = parametersList.OrderBy(s => s[..s.IndexOf("=")]).ToList();
            return (url + "?" + string.Join("&", parametersList.ToArray()));
        }

        public string CreateOAuthAuthorizationToken(
            string url,
            string oauthConsumerKey,
            string oauthConsumerSecret,
            string oauthToken = null,
            string oauthTokenSecret = null,
            string oauthCallback = null/*"oob"*/,
            string oauthVerifier = null,
            Methode mode = Methode.GET,
            IComparer<string> comparer = null,
            Dictionary<string, string> parameters = null)
        {
            var finalParameters = new Dictionary<string, string>();
            if (parameters != null)
            {
                foreach (var pair in parameters)
                {
                    finalParameters.Add(pair.Key, Uri.EscapeDataString(pair.Value ?? ""));
                }
            }
            finalParameters.Add("oauth_consumer_key", oauthConsumerKey);
            finalParameters.Add("oauth_nonce", Nonce());
            finalParameters.Add("oauth_signature_method", "HMAC-SHA1");
            finalParameters.Add("oauth_version", "1.0");
            if (oauthCallback != null)
            {
                finalParameters.Add("oauth_callback", Uri.EscapeDataString(oauthCallback));
            }

            finalParameters.Add("oauth_timestamp", Timestamp());
            if (oauthToken != null)
            {
                finalParameters.Add("oauth_token", oauthToken);
            }

            if (oauthVerifier != null)
            {
                finalParameters.Add("oauth_verifier", oauthVerifier);
            }
            IEnumerable<KeyValuePair<string, string>> orderedParameters = comparer != null ? finalParameters.OrderBy(s => s.Key, comparer) : (IEnumerable<KeyValuePair<string, string>>)finalParameters.OrderBy(s => s.Key);
            string[] parametersList = orderedParameters.Select(pair => string.Format("{0}={1}", pair.Key, pair.Value)).ToArray();
            string parametersStr = string.Join("&", parametersList);

            string baseStr = mode.ToString() + "&" + Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(parametersStr);

            finalParameters.Add("oauth_signature", ComputeHashString(oauthConsumerSecret, oauthTokenSecret, baseStr));
            parametersList = finalParameters.OrderBy(s => s.Key).Select(pair => string.Format(@"{0}=""{1}""", pair.Key, pair.Value)).ToArray();
            return @"realm=""" + url + @"""," + string.Join(",", parametersList);
        }

        public string CreateOAuthAuthorizationHeader(string url,
            string oauthConsumerKey,
            string oauthConsumerSecret,
            string oauthToken = null,
            string oauthTokenSecret = null,
            string oauthCallback = null/*"oob"*/,
            string oauthVerifier = null,
            Methode mode = Methode.GET,
            IComparer<string> comparer = null,
            Dictionary<string, string> parameters = null)
        {
            return @"OAuth " + CreateOAuthAuthorizationToken(url, oauthConsumerKey, oauthConsumerSecret, oauthToken, oauthTokenSecret, oauthCallback, oauthVerifier, mode, comparer, parameters);
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip
            });
        }

        private string ComputeHashString(string oauthConsumerSecret, string oauthTokenSecret, string baseStr)
        {
            /* create the crypto class we use to generate a signature for the request */
            string keySrting = oauthConsumerSecret + "&" + (oauthTokenSecret ?? "");
            /* generate the signature and add it to our parameters */
            byte[] keyBytes = Encoding.UTF8.GetBytes(keySrting);
            HMACSHA1 hashAlgorithm = new(keyBytes);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(baseStr);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);
            string base64StringHash = Convert.ToBase64String(hashBytes);
            return Uri.EscapeDataString(base64StringHash);
        }
    }
}