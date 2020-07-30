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

        public async Task<ITestBuilder> Get(string url)
        {
            return await MakeRequest(url, HttpMethod.Get);
        }

        public async Task<ITestBuilder> Put(string url)
        {
            return await MakeRequest(url, HttpMethod.Put);
        }


        public async Task<ITestBuilder> Post(string url)
        {
            return await MakeRequest(url, HttpMethod.Post);
        }


        public async Task<ITestBuilder> Patch(string url)
        {
            return await MakeRequest(url, new HttpMethod("PATCH"));
        }

        public async Task<ITestBuilder> Delete(string url)
        {
            return await MakeRequest(url, HttpMethod.Delete);
        }

        private async Task<ITestBuilder> MakeRequest(string url, HttpMethod method)
        {
            var builder = TestBuilderFactory.Create(url, _client, useCamelCase: _server.UseCamelCase);
            builder.SetMethod(method);
            return builder;
        }
    }
}
