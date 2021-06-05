using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NSuperTest.Registration;
using NSuperTest.Registration.NetCoreServer;

namespace TestApi.Tests
{
    public class ServerRegistrar : IRegisterServers
    {
        public const string TestServer = "TestServer";
        public const string TestServer2 = "TestServer2";

        public void Register(ServerRegistry reg)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            reg.RegisterNetCoreServer<Startup>(TestServer)
                .WithConfig(config);

            var builder = new WebHostBuilder().UseConfiguration(config.Build());
            reg.RegisterNetCoreServer<Startup>(TestServer2)
                .WithBuilder(builder);

            //reg.RegisterProxyServer("Test2", "https://www.google.com");
        }
    }
}
