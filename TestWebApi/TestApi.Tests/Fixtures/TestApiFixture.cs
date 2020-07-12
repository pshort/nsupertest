
using System;
using Microsoft.Extensions.Configuration;
using NSuperTest;

namespace TestApi.Tests.Fixtures
{
    public class TestApiFixture : IDisposable
    {
        Server<Startup> _server;

        public TestApiFixture()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            _server = new Server<Startup>(config);
        }

        public Server<Startup> Server { get { return _server; } }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}