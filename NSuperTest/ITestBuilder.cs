using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSuperTest
{
    /// <summary>
    /// The main interface for building tests in NSuperTest.
    /// </summary>
    public interface ITestBuilder
    {
        /// <summary>
        /// Expect a HTTP response code
        /// </summary>
        /// <param name="code">The code to expect</param>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder Expect(int code);
        /// <summary>
        /// Expect a HTTP response code and end the chain
        /// </summary>
        /// <param name="code">The code to expect</param>
        /// <param name="callback">A final callback function to end the chain</param>
        void Expect(int code, Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect a HTTP response code
        /// </summary>
        /// <param name="code">The code to expect</param>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder Expect(HttpStatusCode code);
        /// <summary>
        /// Expect a HTTP response code and end the chain
        /// </summary>
        /// <param name="code">The code to expect</param>
        /// <param name="callback">A final callback function to end the chain</param>
        void Expect(HttpStatusCode code, Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect 200 success response code
        /// </summary>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder ExpectOk();
        /// <summary>
        /// Expect 200 success response code and end the chain
        /// </summary>
        /// <param name="callback">A final callback function to end the chain</param>
        void ExpectOk(Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect 201 created response code
        /// </summary>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder ExpectCreated();
        /// <summary>
        /// Expect 201 created response code and end the chain
        /// </summary>
        /// <param name="callback">A final callback function to end the chain</param>
        void ExpectCreated(Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect 404 not found response code
        /// </summary>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder ExpectNotFound();
        /// <summary>
        /// Expect 404 not found response code and end the chain
        /// </summary>
        /// <param name="callback">A final callback function to end the chain</param>
        void ExpectNotFound(Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect 400 bad request response code
        /// </summary>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder ExpectBadRequest();
        /// <summary>
        /// Expect 400 bad request response code and end the chain
        /// </summary>
        /// <param name="callback">A final callback function to end the chain</param>
        void ExpectBadRequest(Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect 401 unauthorized response code
        /// </summary>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder ExpectUnauthorized();
        /// <summary>
        /// Expect 401 unauthorized response code and end the chain
        /// </summary>
        /// <param name="callback">A final callback function to end the chain</param>
        void ExpectUnauthorized(Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect 403 forbidden response code
        /// </summary>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder ExpectForbidden();
        /// <summary>
        /// Expect 403 forbidden response code and end the chain
        /// </summary>
        /// <param name="callback">A final callback function to end the chain</param>
        void ExpectForbidden(Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect 302 redirect response code
        /// </summary>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder ExpectRedirect();
        /// <summary>
        /// Expect 302 redirect response code and end the chain
        /// </summary>
        /// <param name="callback">A final callback function to end the chain</param>
        void ExpectRedirect(Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect a string body
        /// </summary>
        /// <param name="body">The string body to expect in the response</param>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder Expect(string body);
        /// <summary>
        /// Expect a string body and end the chain
        /// </summary>
        /// <param name="body">The string body to expect in the response</param>
        /// <param name="callback">A final callback function to end the chain</param>
        void Expect(string body, Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect an object in the response body
        /// </summary>
        /// <param name="body">The response body as an object</param>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder Expect(object body);
        /// <summary>
        /// Expect an object in the response body and end the chain
        /// </summary>
        /// <param name="body">The response body as an object</param>
        /// <param name="callback">A final callback function to end the chain</param>
        void Expect(object body, Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect a specific header and value in the response
        /// </summary>
        /// <param name="header">The header name expected</param>
        /// <param name="value">The header value expected</param>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder Expect(string header, string value);
        /// <summary>
        /// Expect a specific header and valie in the response and end the chain
        /// </summary>
        /// <param name="header">The header name expected</param>
        /// <param name="value">The header value expected</param>
        /// <param name="callback">A final callback function to end the chain</param>
        void Expect(string header, string value, Action<HttpResponseMessage> callback);

        /// <summary>
        /// Expect with just a callback for custom expectations
        /// </summary>
        /// <param name="callback">The callback function to run. Throw an exception to fail the expectation</param>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder Expect(Action<HttpResponseMessage> callback);
        /// <summary>
        /// Set the request method of the outgoing HTTP request
        /// </summary>
        /// <param name="method">The HTTP method to use, Get, Put, Post or Delete etc</param>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder SetMethod(HttpMethod method);

        /// <summary>
        /// Sends a object in the body of the HTTP request
        /// </summary>
        /// <param name="body">The object to send</param>
        /// <returns>ITestBuilder for chaining</returns>
        ITestBuilder Send(object body);
        /// <summary>
        /// Sends a multipart form content post to the server
        /// </summary>
        /// <param name="content">A multipart form content object</param>
        /// <returns></returns>
        ITestBuilder Send(MultipartFormDataContent content);
        /// <summary>
        /// Set a HTTP header in the outgoing request
        /// </summary>
        /// <param name="header">The header to set</param>
        /// <param name="value">The value to set in the header</param>
        /// <returns>ITestBuilder to chain</returns>
        ITestBuilder Set(string header, string value);
        /// <summary>
        /// Sets a bearer token on the request
        /// </summary>
        /// <param name="token">The token to insert into the request (no Bearer prefix needed)</param>
        /// <returns>ITestBuilder to chain</returns>
        ITestBuilder SetBearerToken(string token);
        /// <summary>
        /// Sets a bearer token using a function that returns a string
        /// </summary>
        /// <param name="generator">The Func that will supply the token</param>
        /// <returns></returns>
        ITestBuilder SetBearerToken(Func<string> generator);

        /// <summary>
        /// Sets a bearer token using an async task
        /// </summary>
        /// <param name="generatorTask">The Func of Task that will supply the token</param>
        /// <returns>ITestBuilder to chain</returns>
        ITestBuilder SetBearerToken(Func<Task<string>> generatorTask);

        /// <summary>
        /// Ends the chain
        /// </summary>
        void End();
        /// <summary>
        /// End the chain with a callback for more expectations
        /// </summary>
        /// <param name="callback">The callback taht contains custom expectations</param>
        void End(Action<HttpResponseMessage> callback);
        /// <summary>
        /// End the chain with a callback and a strong typed body
        /// </summary>
        /// <typeparam name="T">The expected strong type of the body for deserialization</typeparam>
        /// <param name="callback">The callback containing custom expectations</param>
        void End<T>(Action<HttpResponseMessage, T> callback);
    }
}
