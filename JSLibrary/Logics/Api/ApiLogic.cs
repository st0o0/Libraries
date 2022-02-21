using JSLibrary.Logics.Api.Interfaces;
using System;
using System.Net.Http;

namespace JSLibrary.Logics.Api
{
    public abstract class ApiLogic : IApiLogic
    {
        protected ApiLogic(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}