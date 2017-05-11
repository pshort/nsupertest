using System;
using System.Net.Http;

namespace NSuperTest
{
    internal class HttpRequestClient : IHttpRequestClient
    {
        private HttpClientHandler handler;
        private HttpClient client;
        public HttpRequestClient(string baseUri)
        {
            handler = new HttpClientHandler {UseCookies = false};
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(baseUri);
        }

        public HttpResponseMessage MakeRequest(HttpRequestMessage message)
        {
            return client.SendAsync(message).Result;
        }
    }
}
