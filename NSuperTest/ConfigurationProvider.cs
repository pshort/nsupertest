#if NETSTANDARD_2_0
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
                #if NETSTANDARD_2_0
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
                #if NETSTANDARD_2_0
                return configuration[portConfigRoute];
                #else
                return ConfigurationManager.AppSettings[portConfigRoute];
                #endif
            }
        }

        #if NETSTANDARD_2_0
        private IConfigurationRoot configuration;
        public ConfigurationProvider(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }
        #endif

        public ConfigurationProvider()
        {
            #if NETSTANDARD_2_0
            this.configuration = new ConfigurationBuilder().AddJsonFile("nsupertest.json", true).Build();   
            #endif
        }
    }
}