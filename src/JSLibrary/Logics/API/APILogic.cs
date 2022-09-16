using System;
using System.Net.Http;
using JSLibrary.Logics.Api.Interfaces;

namespace JSLibrary.Logics.Api
{
    public abstract class APILogic : IAPILogic
    {
        protected APILogic(HttpClient httpClient)
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
