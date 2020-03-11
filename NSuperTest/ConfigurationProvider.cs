using Microsoft.Extensions.Configuration;

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
                return configuration[startupConfigRoute];
            }
        }

        public string Port
        {
            get
            {
                return configuration[portConfigRoute];
            }
        }

        private IConfigurationRoot configuration;
        public ConfigurationProvider(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public ConfigurationProvider()
        {
            this.configuration = new ConfigurationBuilder().AddJsonFile("nsupertest.json", true).Build();   
        }
    }
}