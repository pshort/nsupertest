using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NSuperTest
{
    public class TestBuilder : ITestBuilder
    {
        private IHttpRequestClient client;
        private HttpRequestMessage request;
        private HttpResponseMessage response;
        private ICollection<Action> assertions;
        private bool useCamelCase;

        public TestBuilder(string uri, IHttpRequestClient client, bool useCamelCase = false)
        {
            this.request = new HttpRequestMessage();
            this.request.RequestUri = new Uri(uri, UriKind.Relative);
            this.client = client;
            this.assertions = new List<Action>();
            this.useCamelCase = useCamelCase;
        }

        public ITestBuilder SetMethod(HttpMethod method)
        {
            this.request.Method = method;
            return this;
        }
        
        public ITestBuilder Expect(int code)
        {
            assertions.Add(() => AssertCode(code, response));
            return this;
        }

        public void Expect(int code, Action<HttpResponseMessage> callback)
        {
            assertions.Add(() => AssertCode(code, response));
            End(callback);
        }

        public ITestBuilder ExpectOk()
        {
            assertions.Add(() => AssertCode(200, response));
            return this;
        }

        public void ExpectOk(Action<HttpResponseMessage> callback)
        {
            assertions.Add(() => AssertCode(200, response));
            End(callback);
        }

        public ITestBuilder ExpectCreated()
        {
            assertions.Add(() => AssertCode(201, response));
            return this;
        }

        public void ExpectCreated(Action<HttpResponseMessage> callback)
        {
            assertions.Add(() => AssertCode(201, response));
            End(callback);
        }

        public ITestBuilder Expect(string body)
        {
            assertions.Add(() => AssertBody(body, response));
            return this;
        }

        public void Expect(string body, Action<HttpResponseMessage> callback)
        {
            assertions.Add(() => AssertBody(body, response));
            assertions.Add(() => RunCallback(callback));
            End();
        }

        public ITestBuilder Expect(object body)
        {
            assertions.Add(() => AssertBody(body, response));
            return this;
        }

        public void Expect(object body, Action<HttpResponseMessage> callback)
        {
            assertions.Add(() => AssertBody(body, response));
            End(callback);
        }

        public ITestBuilder Expect(string header, string value)
        {
            assertions.Add(() => AssertHeader(header, value, response));
            return this;
        }

        public void Expect(string header, string value, Action<HttpResponseMessage> callback)
        {
            assertions.Add(() => AssertHeader(header, value, response));
            assertions.Add(() => RunCallback(callback));
            End();
        }

        public ITestBuilder Expect(Action<HttpResponseMessage> callback)
        {
            assertions.Add(() => RunCallback(callback));
            return this;
        }

        public void End<T>(Action<HttpResponseMessage, T> callback)
        {
            assertions.Add(() => RunStrongTypedCallback<T>(callback));
            End();
        }

        public void End(Action<HttpResponseMessage> callback)
        {
            assertions.Add(() => RunCallback(callback));

            End();
        }

        public void End()
        {
            response = client.MakeRequest(request);

            foreach(var assertion in assertions)
            {
                assertion();
            }

            return;
        }

        private void AssertCode(int expected, HttpResponseMessage message)
        {
            var code = (int)message.StatusCode;
            if (code != expected)
            {
                string error = string.Format("Expected status code {0} but got {1}", expected, code);
                throw new Exception(error);
            }
        }

        private void AssertBody(object expected, HttpResponseMessage message)
        {
            string serializedObj = null;
            if(useCamelCase)
            {
                serializedObj = JsonConvert.SerializeObject(expected, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }
            else
            {
                serializedObj = JsonConvert.SerializeObject(expected);
            }

            AssertBody(serializedObj, message);
        }

        private void AssertBody(string expected, HttpResponseMessage message)
        {
            var body = message.Content.ReadAsStringAsync().Result;
            if(string.Compare(expected, body) != 0)
            {
                string error = string.Format("Expected body '{0}' but got '{1}'", expected, body);
                throw new Exception(error);
            }
        }

        private void AssertHeader(string expectedHeader, string expectedValue, HttpResponseMessage message)
        {
            if(!message.Headers.Contains(expectedHeader))
            {
                string notPresentError = string.Format("Header '{0}' not found on response message", expectedHeader);
                throw new Exception(notPresentError);
            }

            var values = message.Headers.GetValues(expectedHeader);
            if(values.Any(h => string.Compare(h, expectedValue) == 0))
            {
                return;
            }

            string error = string.Format("Header '{0}' not found with value '{1}' on response message", expectedHeader, expectedValue);
            throw new Exception(error);
        }

        private void RunStrongTypedCallback<T>(Action<HttpResponseMessage, T> callback)
        {
            T body;
            try
            {
                body = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                throw new Exception(string.Format("Failed to deserialize body to type {0}", typeof(T).FullName));
            }

            try
            {
                if (callback != null)
                    callback(response, body);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        private void RunCallback(Action<HttpResponseMessage> callback)
        {
            try
            {
                if (callback != null)
                    callback(response);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public ITestBuilder Send(object body)
        {
            var json = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return this;
        }

        public ITestBuilder Send(MultipartFormDataContent content)
        {
            request.Content = content;
            return this;
        }

        public ITestBuilder Set(string header, string value)
        {
            request.Headers.Add(header, value);
            return this;
        }

        public ITestBuilder SetBearerToken(string token)
        {
            request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            return this;
        }

        public ITestBuilder SetBearerToken(Func<string> generator)
        {
            var token = generator();
            request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            return this;
        }

        public ITestBuilder SetBearerToken(Func<Task<string>> generatorTask)
        {
            var token = generatorTask().Result;
            request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            return this;
        }

    }
}
