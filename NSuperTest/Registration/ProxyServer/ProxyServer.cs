using NSuperTest.Server;

namespace NSuperTest.Registration.ProxyServer
{
    public class ProxyServer : IServer
    {
        public string Address { get; }

        public ProxyServer(string address)
        {
            Address = address;
        }

        public IHttpRequestClient GetClient()
        {
            return new HttpRequestClient(Address);
        }
    }
}