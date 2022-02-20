using System;
using System.Net.Http;

namespace JSLibrary.Logics.Api.Interfaces
{
    public interface IApiLogic : IDisposable
    {
        public HttpClient HttpClient { get; }
    }
}