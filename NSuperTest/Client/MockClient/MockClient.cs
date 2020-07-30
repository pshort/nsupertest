using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NSuperTest.Client.MockClient
{
    public class MockClient : IHttpRequestClient
    {
        public async Task<HttpResponseMessage> AsyncMakeRequest(HttpRequestMessage message)
        {
            return await Task.FromResult(new HttpResponseMessage());
        }
    }
}
