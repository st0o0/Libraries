﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Utf8Json;

namespace JSLibrary.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<TValue> ReadFromJsonAsync<TValue>(this HttpContent content, CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync<TValue>(await content.ReadAsStreamAsync());
        }
    }
}