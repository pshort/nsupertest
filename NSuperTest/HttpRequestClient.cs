using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public System.Net.Http.HttpResponseMessage MakeRequest(System.Net.Http.HttpRequestMessage message)
        {
            return client.SendAsync(message).Result;
        }
    }
}
