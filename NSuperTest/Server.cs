using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace NSuperTest
{
    /// <summary>
    /// Allows you to test a server hosting an API. 
    /// </summary>
    public class Server : IDisposable
    {
        private bool disposing;

        /// <summary>
        /// Format string for the url to the server
        /// </summary>
        protected const string UrlFormat = "http://localhost:{0}";

        /// <summary>
        /// The port the server is being hosted on
        /// </summary>
        public int Port { protected set; get; }

        /// <summary>
        /// Was the port taken from configuration
        /// </summary>
        protected bool PortFromConfig { private set; get; }

        /// <summary>
        /// holds the reference to the in memory server
        /// that is running the api under test 
        /// </summary>
        protected IDisposable Target { set; get; }

        /// <summary>
        /// If set to true the naming on the Json formatter of the hosted server is expected to use camel case formatting, instead of pascal case.
        /// </summary>
        public bool UseCamelCase { set; get; }

        /// <summary>
        /// The base address of the hosted endpoint
        /// </summary>
        protected string Address { set; get; }

        /// <summary>
        /// Create a server for testing pointing it to a hosted address
        /// </summary>
        /// <param name="address">The base url of the end point to test</param>
        public Server(string address)
        {
            Address = address;
        }

        /// <summary>
        /// Default contructor, not for use but for child classes to use
        /// </summary>
        public Server()
        {
            var config = new ConfigurationBuilder().AddJsonFile("nsupertest.json", true).Build();   
            // set up a port
            var port = config["nsupertest:port"];

            if (!string.IsNullOrEmpty(port))
            {
                int portInt = 0;
                if (!int.TryParse(port, out portInt))
                    throw new Exception("Please provide a valid port in nsupertest:port app setting");

                if (portInt < 1024)
                    throw new Exception("Please avoid assigning to ports in the well known range");

                PortFromConfig = true;
                Port = portInt;
            }
            else
            {
                PortFromConfig = false;
                Port = GetRandomPort();
            }

            Address = string.Format(UrlFormat, Port);

            // try
            // {
            StartServer(config);
            // }
            // catch (HttpListenerException ex)
            // {
            //     if (PortFromConfig)
            //         throw new Exception(string.Format("The port {0} specified in nsupertest:port is unavailable", Port), ex);

            //     // we clashed ports
            //     Port = GetRandomPort();
            //     Address = string.Format(UrlFormat, Port);
            //     Target = StartServer(config);
            // }
        }

        /// <summary>
        /// Starts the in-memory server
        /// </summary>
        /// <returns>The server</returns>
        protected virtual IDisposable StartServer(IConfigurationRoot config)
        {
            // Todo: change to new configuration system
            var appStartup = config["nsupertest:appstartup"];

            if (string.IsNullOrEmpty(appStartup))
            {
                throw new Exception("Please provide a server start class using the nsupertest:appStartup app setting");
            }

            

            try
            {
                //return WebApp.Start(options);
                var host = new WebHostBuilder()
                            .UseKestrel()
                            .UseUrls(new string[] {Address})
                            .UseStartup(appStartup)
                            .Build();

                host.Start();
                return host;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to load the type specified in nsupertest:appStartup", ex);
            }
        }

        /// <summary>
        /// Get a port int between 1024 and 51024. should be ok..
        /// </summary>
        /// <returns></returns>
        protected int GetRandomPort()
        {
            var random = new System.Random(DateTime.Now.Millisecond);
            return random.Next(50000) + 1024;
        }

        /// <summary>
        /// Make a Get HTTP request to the server
        /// </summary>
        /// <param name="url">The url to Get. Must start with /.</param>
        /// <returns>ITestBuilder to chain assertions</returns>
        public ITestBuilder Get(string url)
        {
            return Request(HttpMethod.Get, url);
        }

        /// <summary>
        /// Make a Post HTTP request to the server
        /// </summary>
        /// <param name="url">The url to Post to. Must start with /.</param>
        /// <returns>ITestBuilder to chain assertions</returns>
        public ITestBuilder Post(string url)
        {
            return Request(HttpMethod.Post, url);
        }

        /// <summary>
        /// Make a Put HTTP request to the server
        /// </summary>
        /// <param name="url">The url to Put to. Must start with /.</param>
        /// <returns>ITestBuilder to chain assertions</returns>
        public ITestBuilder Put(string url)
        {
            return Request(HttpMethod.Put, url);
        }

        /// <summary>
        /// Make a Patch HTTP request to the server
        /// </summary>
        /// <param name="url">The url to Patch to. Must start with /.</param>
        /// <returns>ITestBuilder to chain assertions</returns>
        public ITestBuilder Patch(string url)
        {
            var patch = new HttpMethod("PATCH");
            return Request(patch, url);
        }

        /// <summary>
        /// Make a Delete HTTP request to the server
        /// </summary>
        /// <param name="url">The url to send Delete to. Must start with /.</param>
        /// <returns>ITestBuilder to chain assertions</returns>
        public ITestBuilder Delete(string url)
        {
            return Request(HttpMethod.Delete, url);
        }

        /// <summary>
        /// Make an HTTP request to the server
        /// </summary>
        /// <param name="method">The HTTP method to use</param>
        /// <param name="url">The url to send the request to. Must start with /.</param>
        /// <returns>ITestBuilder to chain assertions</returns>
        public ITestBuilder Request(HttpMethod method, string url)
        {
            var client = new HttpRequestClient(Address);
            var builder = TestBuilderFactory.Create(url, client, useCamelCase: UseCamelCase);
            builder.SetMethod(method);
            return builder;
        }

        /// <summary>
        /// Clean up in memory resource and tear down any servers..
        /// </summary>
        public void Dispose()
        {
            if(!disposing)
            {
                disposing = true;

                if (Target != null)
                {
                    Target.Dispose();
                    Target = null;
                }

                disposing = false;
            }
        }

        /// <summary>
        /// Clean up allocated resources
        /// </summary>
        ~Server()
        {
            this.Dispose();
        }
    }
    /// <summary>
    /// An object to create an in memery api server for testing Apis.
    /// </summary>
    /// <typeparam name="T">The Startup class of the API server</typeparam>
    public class Server<T> : Server where T : class
    {
        /// <summary>
        /// Create a new server
        /// </summary>
        public Server()
            : base()
        {
        }

        /// <summary>
        /// Starts the in-memory server
        /// </summary>
        /// <returns>The server</returns>
        protected override IDisposable StartServer(IConfigurationRoot config)
        {
            var host = new WebHostBuilder()
                        .UseKestrel()
                        .UseUrls(new string[] { Address })
                        .UseStartup<T>()
                        .Build();
            host.Start();

            return host;
        }
    }
}
