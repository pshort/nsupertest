using System;
using Microsoft.Extensions.Configuration;
using NSuperTest.Registration;
using NSuperTest.Registration.NetCoreServer;
using NSuperTest.Registration.ProxyServer;

namespace TestApi.Tests
{
    public class ServerRegistrar : IRegisterServers
    {
        public void Register(ServerRegistry reg)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            reg.RegisterNetCoreServer<Startup>("TestServer", config);

            reg.RegisterProxyServer("Test2", "https://www.google.com");
        }
    }
}
