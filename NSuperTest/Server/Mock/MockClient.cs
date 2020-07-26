using System.Net.Http;
using System.Threading.Tasks;
using NSuperTest.Messaging;
using NSuperTest.Messaging.Mock;

namespace NSuperTest.Server.Mock
{
    public class MockClient : IRequestClient
    {
        public Task<IClientResponse> AsyncMakeRequest()
        {
            return Task.FromResult<IClientResponse>(new MockClientResponse());
        }
    }
}