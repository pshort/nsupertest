#if NETCOREAPP_2_1
using Microsoft.Extensions.Configuration;
#endif

#if NETFULL
using System.Configuration;
#endif

namespace NSuperTest
{
    public class ConfigurationProvider
    {
        const string portConfigRoute = "nsupertest:port";
        const string startupConfigRoute = "nsupertest:appstartup";

        public string ServerClass
        {
            get
            {
                #if NETCOREAPP_2_1
                return configuration[startupConfigRoute];
                #else
                return ConfigurationManager.AppSettings[startupConfigRoute];
                #endif
            }
        }
        public string Port
        {
            get
            {
                #if NETCOREAPP_2_1
                return configuration[portConfigRoute];
                #else
                return ConfigurationManager.AppSettings[portConfigRoute];
                #endif
            }
        }

        #if NETCOREAPP_2_1
        private IConfigurationRoot configuration;
        public ConfigurationProvider(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }
        #endif

        public ConfigurationProvider()
        {
            #if NETCOREAPP_2_1
            this.configuration = new ConfigurationBuilder().AddJsonFile("nsupertest.json", true).Build();   
            #endif
        }
    }
}