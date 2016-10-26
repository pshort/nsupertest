using Microsoft.Owin.Host.HttpListener;
using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;

namespace NSuperTest
{
    /// <summary>
    /// Allows you to test a server hosting an API. 
    /// </summary>
    public class Server : IDisposable
    {
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
        protected IDisposable Target
        {
            set; get;
        }

        /// <summary>
        /// If set to true the naming on the Json formatter of the hosted server is expected to use camel case formatting, instead of pascal case.
        /// </summary>
        public bool UseCamelCase { set; get; }

        /// <summary>
        /// The base address of the hosted endpoint
        /// </summary>
        protected string Address
        {
            get;
            set;
        }

        /// <summary>
        /// Create a server for testing pointing it to a hosted address
        /// </summary>
        /// <param name="address">The base url of the end point to test</param>
        public Server(string address)
        {
            Address = address;
            //UseCamelCase = true;
        }

        /// <summary>
        /// Default contructor, not for use but for child classes to use
        /// </summary>
        public Server()
        {
            //UseCamelCase = true;
            // this code is ensure the httplistener lib gets onto build servers
            var listener = typeof(OwinHttpListener);
            if (listener != null) { }
            // end of rediculous hacky code to ensure builds come with the owinhttplistener dll

            // set up a port
            var port = ConfigurationManager.AppSettings["nsupertest:Port"];

            if (!string.IsNullOrEmpty(port))
            {
                int portInt = 0;
                if (!int.TryParse(port, out portInt))
                    throw new ApplicationException("Please provide a valid port in nsupertest:port app setting");

                if (portInt < 1024)
                    throw new ApplicationException("Please avoid assigning to ports in the well known range");

                PortFromConfig = true;
                Port = portInt;
            }
            else
            {
                PortFromConfig = false;
                Port = GetRandomPort();
            }

            Address = string.Format(UrlFormat, Port);

            try
            {
                Target = StartServer();
            }
            catch (HttpListenerException ex)
            {
                if (PortFromConfig)
                    throw new ApplicationException(string.Format("The port {0} specified in nsupertest:port is unavailable", Port), ex);

                // we clashed ports
                Port = GetRandomPort();
                Address = string.Format(UrlFormat, Port);
                Target = StartServer();
            }
        }

        protected virtual IDisposable StartServer()
        {
            var appStartup = ConfigurationManager.AppSettings["nsupertest:appStartup"];

            if (string.IsNullOrEmpty(appStartup))
            {
                throw new ApplicationException("Please provide a server start class using the nsupertest:appStartup app setting");
            }

            try
            {
                var type = Type.GetType(appStartup, true, false);
                var options = new StartOptions
                {
                    AppStartup = type.FullName,
                    Port = Port
                };
                return WebApp.Start(options);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to load the type specified in nsupertest:appStartup", ex);
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
            var builder = new TestBuilder(url, client, UseCamelCase);
            builder.SetMethod(method);
            return builder;
        }

        /// <summary>
        /// Clean up in memory resource and tear down any servers..
        /// </summary>
        public void Dispose()
        {
            if(!_disposing)
            {
                _disposing = true;

                if (Target != null)
                {
                    Target.Dispose();
                    Target = null;
                }

                _disposing = false;
            }
        }

        /// <summary>
        /// Clean up allocated resources
        /// </summary>
        ~Server()
        {
            this.Dispose();
        }

        private bool _disposing;
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
            //UseCamelCase = false;
        }

        /// <summary>
        /// Starts the in-memory server
        /// </summary>
        /// <returns>The server</returns>
        protected override IDisposable StartServer()
        {
            return WebApp.Start<T>(Address);
        }
    }
}
