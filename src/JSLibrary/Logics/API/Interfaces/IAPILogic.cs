using System;
using System.Net.Http;

namespace JSLibrary.Logics.Api.Interfaces
{
    public interface IAPILogic : IDisposable
    {
        public HttpClient HttpClient { get; }
    }
}