using System;
using System.Net.Http;

namespace JSLibrary.Logics.Api.Interfaces
{
    public interface IApiLogic<HttpClientFactoryType> : IDisposable where HttpClientFactoryType : IHttpClientFactory
    {
        public HttpClient HttpClient { get; }
    }
}