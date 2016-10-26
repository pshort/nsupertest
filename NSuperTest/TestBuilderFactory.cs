using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSuperTest
{
    /// <summary>
    /// A factory to create ITestBuilder instances
    /// </summary>
    public static class TestBuilderFactory
    {
        /// <summary>
        /// Create an instance of an ITestBuilder implementation
        /// </summary>
        /// <param name="url">The url for the test builder</param>
        /// <param name="client">The http client to use for making http requests.</param>
        /// <param name="useCamelCase">Formatter setting for whether or not to use camel casing</param>
        /// <returns>An instance of an ITestBuilder implementation</returns>
        public static ITestBuilder Create(string url, IHttpRequestClient client, bool useCamelCase = false)
        {
            return new TestBuilder(url, client, useCamelCase);
        }
    }
}
