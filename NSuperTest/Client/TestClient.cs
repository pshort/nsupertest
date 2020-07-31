using NSuperTest.Server;
using System.Net.Http;
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

        public ITestBuilder Put(string url)
        {
            return MakeRequest(url, HttpMethod.Put);
        }

        public ITestBuilder Post(string url)
        {
            return MakeRequest(url, HttpMethod.Post);
        }

        public ITestBuilder Patch(string url)
        {
            return MakeRequest(url, new HttpMethod("PATCH"));
        }

        public ITestBuilder Delete(string url)
        {
            return MakeRequest(url, HttpMethod.Delete);
        }

        private ITestBuilder MakeRequest(string url, HttpMethod method)
        {
            var builder = TestBuilderFactory.Create(url, _client, useCamelCase: _server.UseCamelCase);
            builder.SetMethod(method);
            return builder;
        }
    }
}
