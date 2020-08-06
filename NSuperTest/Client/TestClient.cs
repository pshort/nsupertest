using Newtonsoft.Json;
using NSuperTest.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NSuperTest.Client
{
    public class Headers : Dictionary<string, string> { }
    public class Query : Dictionary<string, string> { }

    public class TestClient
    {
        private IHttpRequestClient _client;
        private IServer _server;
        private RequestBuilder _requestBuilder;

        public TestClient(string serverName)
        {
            _server = ServerFactory.Instance.Build(serverName);
            _client = _server.GetClient();
            _requestBuilder = new RequestBuilder();
        }

        public async Task<HttpResponseMessage> GetAsync(
            string url,
            Headers headers = null,
            Query query = null
        )
        {
            return await MakeRequestAsync(url, HttpMethod.Get, headers: headers, query: query);
        }

        public async Task<HttpResponseMessage> PutAsync(
            string url, 
            object body,
            Headers headers = null,
            Query query = null
        )
        {
            return await MakeRequestAsync(url, HttpMethod.Put, body, headers, query);
        }

        public async Task<HttpResponseMessage> PostAsync(
            string url, 
            object body, 
            Headers headers = null, 
            Query query = null
        )
        {
            return await MakeRequestAsync(url, HttpMethod.Post, body, headers, query);
        }

        public async Task<HttpResponseMessage> PatchAsync(
            string url, 
            object body,
            Headers headers = null,
            Query query = null
        )
        {
            return await MakeRequestAsync(url, new HttpMethod("PATCH"), body, headers, query);
        }

        public async Task<HttpResponseMessage> DeleteAsync(
            string url,
            Headers headers = null,
            Query query = null
        )
        {
            return await MakeRequestAsync(url, HttpMethod.Delete, headers: headers, query: query);
        }

        public async Task<HttpResponseMessage> MakeRequestAsync(Request request)
        {
            return await MakeRequestAsync(request.Url, request.Method, request.Body, request.Headers, request.Query);
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(
            string url, 
            HttpMethod method, 
            object body = null,
            Headers headers = null,
            Query query = null
        )
        {
            var request = _requestBuilder.Build(url, method, body, headers, query);
            return await _client.AsyncMakeRequest(request);
        }
    }
}
