using NSuperTest.Server;

namespace NSuperTest.Registration.ProxyServer
{
    public class ProxyServerBuilder : IServerBuilder
    {
        private string _address;
        public ProxyServerBuilder(string address)
        {
            _address = address;
        }

        public IServer Build()
        {
            return new ProxyServer(_address);
        }
    }
}