using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Registration.NetCoreServer
{
    public static class RegistryExtension
    {
        public static void RegisterNetCoreServer<T>(this ServerRegistry reg, string name)
            where T : class
        {
            var netCoreServerBuilder = new NetCoreServerBuilder<T>();
            reg.Register(name, netCoreServerBuilder);
        }

        public static void RegisterNetCoreServer<T>(this ServerRegistry reg, string name, IConfigurationBuilder config)
            where T : class
        {
            var netCoreServerBuilder = new NetCoreServerBuilder<T>();
            netCoreServerBuilder.WithConfig(config);
            reg.Register(name, netCoreServerBuilder);
        }

        public static void RegisterNetCoreServer<T>(this ServerRegistry reg, string name, IWebHostBuilder hostBuilder)
            where T : class
        {
            var netCoreServerBuilder = new NetCoreServerBuilder<T>();

            netCoreServerBuilder.WithBuilder(hostBuilder);
            reg.Register(name, netCoreServerBuilder);

        }
    }
}
