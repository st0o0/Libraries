﻿using JSLibrary.Logics.Api.Interfaces;
using System;
using System.Net.Http;

namespace JSLibrary.Logics.Api
{
    public class ApiLogic<HttpClientFactoryType> : IApiLogic<HttpClientFactoryType> where HttpClientFactoryType : IHttpClientFactory
    {
        public ApiLogic(string httpClientName, HttpClientFactoryType hCContext)
        {
            HttpClient = hCContext.CreateClient(httpClientName);
        }

        public ApiLogic(HttpClient httpClient)
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