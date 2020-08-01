using Newtonsoft.Json;
using NSuperTest.Server;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NSuperTest.Client
{
    public class TestClient
    {
        private IHttpRequestClient _client;
        private IServer _server;

        public TestClient(string serverName)
        {
            _server = ServerFactory.Instance.Build(serverName);
            _client = _server.GetClient();
        }

        public ITestBuilder Get(string url)
        {
            return MakeRequest(url, HttpMethod.Get);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await MakeRequestAsync(url, HttpMethod.Get);
        }

        public ITestBuilder Put(string url)
        {
            return MakeRequest(url, HttpMethod.Put);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, object body)
        {
            return await MakeRequestAsync(url, HttpMethod.Put, body);
        }

        public ITestBuilder Post(string url)
        {
            return MakeRequest(url, HttpMethod.Post);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object body)
        {
            return await MakeRequestAsync(url, HttpMethod.Post, body);
        }

        public ITestBuilder Patch(string url)
        {
            return MakeRequest(url, new HttpMethod("PATCH"));
        }

        public async Task<HttpResponseMessage> PatchAsync(string url, object body)
        {
            return await MakeRequestAsync(url, new HttpMethod("PATCH"), body);
        }

        public ITestBuilder Delete(string url)
        {
            return MakeRequest(url, HttpMethod.Delete);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await MakeRequestAsync(url, HttpMethod.Delete);
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(string url, HttpMethod method, object body = null)
        {
            var uri = new Uri(url, UriKind.Relative);
            var req = new HttpRequestMessage(method, uri);

            if(body != null && IsBodiedRequest(method))
            {
                var bodySerialized = JsonConvert.SerializeObject(body);
                req.Content = new StringContent(bodySerialized, Encoding.UTF8, "application/json");
            }

            return await _client.AsyncMakeRequest(req);
        }

        private bool IsBodiedRequest(HttpMethod method)
        {
            return method == HttpMethod.Post || method == HttpMethod.Put || method.Method == "PATCH";
        }

        private ITestBuilder MakeRequest(string url, HttpMethod method)
        {
            var builder = TestBuilderFactory.Create(url, _client, useCamelCase: _server.UseCamelCase);
            builder.SetMethod(method);
            return builder;
        }
    }
}
