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

        public TestClient(string serverName)
        {
            _server = ServerFactory.Instance.Build(serverName);
            _client = _server.GetClient();
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
            if(query != null && query.Count != 0)
            {
                var sb = new StringBuilder(url);
                if(url.Contains("?"))
                {
                    foreach(var q in query)
                    {
                        sb.Append($"&{q.Key}={q.Value}");
                    }
                }
                else
                {
                    var f = query.First();
                    sb.Append($"?{f.Key}={f.Value}");
                    foreach(var q in query.Skip(1))
                    {
                        sb.Append($"&{q.Key}={q.Value}");
                    }
                }
                url = sb.ToString();
            }

            var uri = new Uri(url, UriKind.Relative);
            var req = new HttpRequestMessage(method, uri);

            if(body != null && IsBodiedRequest(method))
            {
                var bodySerialized = JsonConvert.SerializeObject(body);
                req.Content = new StringContent(bodySerialized, Encoding.UTF8, "application/json");
            }

            if(headers != null && headers.Count != 0)
            {
                foreach(var header in headers)
                {
                    req.Headers.Add(header.Key, header.Value);
                }
            }

            return await _client.AsyncMakeRequest(req);
        }

        private bool IsBodiedRequest(HttpMethod method)
        {
            return method == HttpMethod.Post || method == HttpMethod.Put || method.Method == "PATCH";
        }
    }
}
