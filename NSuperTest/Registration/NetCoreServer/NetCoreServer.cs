using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using NSuperTest.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSuperTest.Registration.NetCoreServer
{
    public class NetCoreServer : IServer
    {
        private IWebHost _host;

        public string Address { get; }

        public NetCoreServer(IWebHost host)
        {
            _host = host;
            var addresses = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            Address = addresses.First();
        }

        public IHttpRequestClient GetClient()
        {
            var client = new HttpRequestClient(Address);
            return client;
        }

        public IServiceProvider GetServices()
        {
            return _host.Services;
        }
    }
}
