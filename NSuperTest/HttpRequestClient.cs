using System;
using System.Net.Http;

namespace NSuperTest
{
    public class HttpRequestClient : IHttpRequestClient
    {
        private HttpClient client;
        public HttpRequestClient(string baseUri)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
        }

        public HttpResponseMessage MakeRequest(HttpRequestMessage message)
        {
            return client.SendAsync(message).Result;
        }
    }
}
