using Microsoft.Owin.Host.HttpListener;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NSuperTest
{
    /// <summary>
    /// Allows you to test a server hosting an API. 
    /// </summary>
    public class Server
    {
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
            UseCamelCase = true;
        }

        /// <summary>
        /// Default contructor, not for use but for child classes to use
        /// </summary>
        protected Server() { UseCamelCase = true; }

        /// <summary>
        /// Make a Get HTTP request to the server
        /// </summary>
        /// <param name="url">The url to Get starting with /</param>
        /// <returns>ITestBuilder to chain assertions</returns>
        public ITestBuilder Get(string url)
        {
            return GetBuilder(HttpMethod.Get, url);
        }

        /// <summary>
        /// Make a Post HTTP request to the server
        /// </summary>
        /// <param name="url">The url to Post to starting with /</param>
        /// <returns>ITestBuilder to chain assertions</returns>
        public ITestBuilder Post(string url)
        {
            return GetBuilder(HttpMethod.Post, url);
        }

        /// <summary>
        /// Make a Put HTTP request to the server
        /// </summary>
        /// <param name="url">The url to Put to starting with /</param>
        /// <returns>ITestBuilder to chain assertions</returns>
        public ITestBuilder Put(string url)
        {
            return GetBuilder(HttpMethod.Put, url);
        }

        /// <summary>
        /// Make a Delete HTTP request to the server
        /// </summary>
        /// <param name="url">The url to send Delete to starting with /</param>
        /// <returns>ITestBuilder to chain assertions</returns>
        public ITestBuilder Delete(string url)
        {
            return GetBuilder(HttpMethod.Delete, url);
        }

        private ITestBuilder GetBuilder(HttpMethod method, string url)
        {
            var client = new HttpRequestClient(Address);
            var builder = new TestBuilder(url, client, UseCamelCase);
            builder.SetMethod(method);
            return builder;
        }
    }
    /// <summary>
    /// An object to create an in memery api server for testing Apis.
    /// </summary>
    /// <typeparam name="T">The Startup class of the API server</typeparam>
    public class Server<T> : Server where T : class
    {
        private const string UrlFormat = "http://localhost:{0}";
        /// <summary>
        /// The port the server is being hosted on
        /// </summary>
        public int Port { private set; get; }
        // holds the reference to the in memory server
        // that is running the api under test
        private IDisposable Target { set; get; }
        
        /// <summary>
        /// Create a new server
        /// </summary>
        public Server()
        {
            // this code is ensure the httplistener lib gets onto build servers
            var listener = typeof(OwinHttpListener);
            if (listener != null) { }
            // end of rediculous hacky code to ensure builds come with the owinhttplistener dll

            Port = 3105;
            Address = string.Format(UrlFormat, Port);
            Target = WebApp.Start<T>(Address);
            UseCamelCase = false;
        }
        
        /// <summary>
        /// Clean up in memory resource and tear down any servers..
        /// </summary>
        ~Server()
        {
            if(Target != null)
            {
                Target.Dispose();
                Target = null;
            }
        }
    }
}
