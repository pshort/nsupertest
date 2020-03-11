using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;


namespace NSuperTest
{
    public abstract class ServerBase : IDisposable
    {
        protected ConfigurationProvider configuration;

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

        protected void RunServer()
        {
            configuration = new ConfigurationProvider();
            SetAddress();
            try
            {
                Target = StartServer();
            }
            catch (HttpListenerException ex)
            {
                if (PortFromConfig)
                    throw new Exception(string.Format("The port {0} specified in nsupertest:port is unavailable", Port), ex);

                SetAddress();
                Target = StartServer();
            }
        }


        /// <summary>
        /// Starts the in-memory server
        /// </summary>
        /// <returns>The server</returns>
        protected virtual IDisposable StartServer()
        {
            // Todo: change to new configuration system
            var appStartup = configuration.ServerClass;

            if (string.IsNullOrEmpty(appStartup))
            {
                Throw("Please provide a server start class or Assembly if netcore using the nsupertest:appStartup app setting");
            }

            try
            {

                var host = new WebHostBuilder()
                            .UseKestrel()
                            .UseUrls(new string[] { Address })
                            .UseStartup(appStartup)
                            .Build();
                host.Start();

                return host;
            }
            catch (Exception ex)
            {
                Throw("Unable to load the type specified in nsupertest:appStartup", ex);
            }

            return null;
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

        protected void SetAddress()
        {
            string port = configuration.Port;

            if (!string.IsNullOrEmpty(port))
            {
                int portInt = 0;
                if (!int.TryParse(port, out portInt))
                    Throw("Please provide a valid port in nsupertest:port app setting");

                if (portInt < 1024)
                    Throw("Please avoid assigning to ports in the well known range");

                PortFromConfig = true;
                Port = portInt;
            }
            else
            {
                PortFromConfig = false;
                Port = GetRandomPort();
            }

            Address = string.Format(UrlFormat, Port);
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
            if (!disposing)
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

        private void Throw(string message)
        {
            throw new Exception(message);
        }

        private void Throw(string message, Exception ex)
        {
            throw new Exception(message, ex);
        }

        /// <summary>
        /// Clean up allocated resources
        /// </summary>
        ~ServerBase()
        {
            this.Dispose();
        }

    }
}
