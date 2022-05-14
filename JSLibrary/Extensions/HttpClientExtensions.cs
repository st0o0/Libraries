using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utf8Json;

namespace JSLibrary.Extensions
{
    public static class HttpClientExtensions
    {
        // Get
        public static async Task<TValue> GetFromJsonAsync<TValue>(this HttpClient httpClient, string requestUri, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri), cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<TValue>(cancellationToken);
        }

        public static async Task<TValue> GetFromJsonAsync<TValue>(this HttpClient httpClient, Uri requestUri, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri), cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<TValue>(cancellationToken);
        }

        // Post

        public static async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient httpClient, string requestUri, TValue value, CancellationToken cancellationToken = default)
        {
            return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = Serialize(value) }, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient httpClient, Uri requestUri, TValue value, CancellationToken cancellationToken = default)
        {
            return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = Serialize(value) }, cancellationToken);
        }

        // Put

        public static async Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient httpClient, string requestUri, TValue value, CancellationToken cancellationToken = default)
        {
            return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = Serialize(value) }, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient httpClient, Uri requestUri, TValue value, CancellationToken cancellationToken = default)
        {
            return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = Serialize(value) }, cancellationToken);
        }

        // Delete
        public static async Task<HttpResponseMessage> DeleteAsJsonAsync<TValue>(this HttpClient httpClient, string requestUri, TValue value, CancellationToken cancellationToken = default)
        {
            return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(value) }, cancellationToken);
        }

        public static async Task<HttpResponseMessage> DeleteAsJsonAsync<TValue>(this HttpClient httpClient, Uri requestUri, TValue value, CancellationToken cancellationToken = default)
        {
            return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(value) }, cancellationToken);
        }

        // utf8json Serialize
        private static HttpContent Serialize<TValue>(TValue data) => new StringContent(JsonSerializer.ToJsonString(data), Encoding.UTF8, "application/json");
    }
}