using System;
using System.Net;
using System.Net.Http;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;


namespace NSuperTest
{
    /// <summary>
    /// An object to create an in memery api server for testing Apis.
    /// </summary>
    /// <typeparam name="T">The Startup class of the API server</typeparam>
    public class Server<T> : ServerBase where T : class
    {
        /// <summary>
        /// Create a new server
        /// </summary>
        public Server()
        {
            Console.WriteLine("Starting generic server with no args");
            RunServer();
        }

        /// <summary>
        /// Create a server for testing pointing it to a hosted address
        /// </summary>
        /// <param name="address">The base url of the end point to test</param>
        public Server(string address)
        {
            Address = address;
        }

        protected new IConfigurationBuilder configuration;
        public Server(IConfigurationBuilder builder)
        {
            Console.WriteLine("Starting generic server with config builder");
            this.configuration = builder;
            base.RunServer();
        }

        protected new IWebHostBuilder builder;
        /// <summary>
        /// Create a new server by supplying the builder yourself. The Build call will be called by the server, and Kestrel and a
        /// private address will be used, but otherwise all options are left to you.
        /// </summary>
        /// <param name="builder">A web host builder to configure the server</param>
        public Server(IWebHostBuilder builder)
        {
            this.builder = builder;
            base.RunServer();
        }

        /// <summary>
        /// Starts the in-memory server
        /// </summary>
        /// <returns>The server</returns>
        protected override IDisposable StartServer()
        {
            IWebHost host;
            if(this.builder != null)
            {
                Console.WriteLine("Using a custom builder");
                host = builder
                        .UseKestrel()
                        .UseUrls(new string[] {  Address })
                        .UseStartup<T>()
                        .Build();
            }
            else if (this.configuration != null)
            {
                Console.WriteLine("Config not null, hooking into webhost builder");
                host = new WebHostBuilder()
                        .UseConfiguration(configuration.Build())
                        .UseKestrel()
                        .UseUrls(new string[] { Address })
                        .UseStartup<T>()
                        .Build();
            }
            else
            {
                Console.WriteLine("Config is null, generic webhost builder");
                host = new WebHostBuilder()
                        .UseKestrel()
                        .UseUrls(new string[] { Address })
                        .UseStartup<T>()
                        .Build();
            }

            host.Start();
            return host;
        }


    }
}
