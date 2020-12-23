using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NSuperTest.Registration;
using NSuperTest.Registration.NetCoreServer;
using NSuperTest.Registration.ProxyServer;

namespace TestApi.Tests
{
    public class ServerRegistrar : IRegisterServers
    {
        public const string TestServer = "TestServer";
        public const string TestServer2 = "TestServer2";

        public void Register(ServerRegistry reg)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            reg.RegisterNetCoreServer<Startup>(TestServer, config);

            reg.RegisterNetCoreServer<Startup>(TestServer2, new WebHostBuilder().UseConfiguration(config.Build()));

            reg.RegisterProxyServer("Test2", "https://www.google.com");
        }
    }
}
