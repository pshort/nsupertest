using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NSuperTest.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Registration.NetCoreServer
{
    public class NetCoreServerBuilder<T> : IServerBuilder
        where T : class
    {
        private static object _outer = new object();

        private static IWebHost _host = null;
        private static IWebHostBuilder _builder = null;
        private const string _httpUrl = "http://[::1]:0";
        private const string _httpsUrl = "https://[::1]:0";

        public NetCoreServerBuilder()
        {
            _builder = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(new string[] { _httpUrl })
                .UseStartup<T>();
        }

        public void WithConfig(IConfigurationBuilder configBuilder)
        {
            var config = configBuilder.Build();
            _builder.UseConfiguration(config);
        }

        public IServer Build()
        {
            lock(_outer)
            {
                if(_host == null)
                {
                    _host = _builder.Build();
                    _host.Start();
                }
            }

            return new NetCoreServer(_host);
        }
    }
}
