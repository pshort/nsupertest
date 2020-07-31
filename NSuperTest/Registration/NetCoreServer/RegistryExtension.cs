using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Registration.NetCoreServer
{
    public static class RegistryExtension
    {
        public static void RegisterNetCoreServer<T>(this ServerRegistry reg, string name, IConfigurationBuilder config = null)
            where T : class
        {
            var netCoreServerBuilder = new NetCoreServerBuilder<T>();
            if(config != null)
            {
                netCoreServerBuilder.WithConfig(config);
            }
            reg.Register(name, netCoreServerBuilder);
        }
    }
}
