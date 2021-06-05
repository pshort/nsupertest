using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NSuperTest.Registration.NetCoreServer
{
    public static class RegistryExtension
    {
        public static NetCoreServerBuilder<T> RegisterNetCoreServer<T>(this ServerRegistry reg, string name)
            where T : class
        {
            var netCoreServerBuilder = new NetCoreServerBuilder<T>();
            reg.Register(name, netCoreServerBuilder);
            return netCoreServerBuilder;
        }

        public static NetCoreServerBuilder<T> WithConfig<T>(this NetCoreServerBuilder<T> builder, IConfigurationBuilder config)
            where T : class
        {
            builder.WithConfig(config);
            return builder;
        }

        public static NetCoreServerBuilder<T> WithBuilder<T>(this NetCoreServerBuilder<T> builder,
            IWebHostBuilder webHostBuilder)
            where T : class
        {
            builder.WithBuilder(webHostBuilder);
            return builder;
        }

        // public static NetCoreServerBuilder<T> RegisterNetCoreServer<T>(this ServerRegistry reg, string name, IConfigurationBuilder config)
        //     where T : class
        // {
        //     var netCoreServerBuilder = new NetCoreServerBuilder<T>();
        //     netCoreServerBuilder.WithConfig(config);
        //     reg.Register(name, netCoreServerBuilder);
        //     return netCoreServerBuilder;
        // }
        //
        // public static void RegisterNetCoreServer<T>(this ServerRegistry reg, string name, IWebHostBuilder hostBuilder)
        //     where T : class
        // {
        //     var netCoreServerBuilder = new NetCoreServerBuilder<T>();
        //
        //     netCoreServerBuilder.WithBuilder(hostBuilder);
        //     reg.Register(name, netCoreServerBuilder);
        //
        // }
    }
}
