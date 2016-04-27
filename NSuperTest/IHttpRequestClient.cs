using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NSuperTest
{
    /// <summary>
    /// Makes http requests.
    /// </summary>
    public interface IHttpRequestClient
    {
        /// <summary>
        /// Make a single HttpRequest. Takes in a request message and returns a response
        /// </summary>
        /// <param name="message">A HttpRequestMessage object</param>
        /// <returns>The associated HttpResponseMessage</returns>
        HttpResponseMessage MakeRequest(HttpRequestMessage message);
    }
}
