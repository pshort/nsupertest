using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NSuperTest.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace NSuperTest.Assertions
{
    public static class HttpAssertionExtensions
    {
        public static void AssertStatusCode(this HttpResponseMessage message, int expected)
        {
            var code = (int)message.StatusCode;
            if (code != expected)
            {
                var expectedHttpStatusCode = (HttpStatusCode)expected;
                var recievedMsg = string.Format("{0} ({1})", message.StatusCode, code);
                var expectedMsg = string.Format("{0} ({1})", expectedHttpStatusCode, expected);

                string error = string.Format("Expected status code {0} but got {1}", expectedMsg, recievedMsg);
                throw new Exception(error);
            }
        }

        public static void AssertBody(this HttpResponseMessage message, object expected, bool useCamelCase = true)
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

            message.AssertBody(serializedObj);
        }

        public static void AssertBody(this HttpResponseMessage message, string expected)
        {
            var body = message.Content.ReadAsStringAsync().Result;
            if(string.Compare(expected, body) != 0)
            {
                string error = string.Format("Expected body '{0}' but got '{1}'", expected, body);
                throw new Exception(error);
            }
        }

        public static void AssertHeader(this HttpResponseMessage message, string expectedHeader, string expectedValue)
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

        public static void AssertBadRequest(this HttpResponseMessage message, Action<ErrorList> callback)
        {
            message.AssertStatusCode(400);
            var body = DeserializeBody<BadRequestResponse>(message.Content.ReadAsStringAsync().Result);

            try
            {
                if (callback != null)
                    callback(body.Errors);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        private static T DeserializeBody<T>(string body)
        {
            T bodyObject;

            try
            {
                bodyObject = JsonConvert.DeserializeObject<T>(body);
            }
            catch
            {
                throw new Exception(string.Format("Failed to deserialize body to type {0}", typeof(T).FullName));
            }

            return bodyObject;
        }

        public static void Run<T>(this HttpResponseMessage message, Action<T> callback)
        {
            var body = DeserializeBody<T>(message.Content.ReadAsStringAsync().Result);

            try
            {
                if (callback != null)
                    callback(body);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public static void Run(this HttpResponseMessage message, Action<HttpResponseMessage> callback)
        {
            try
            {
                if (callback != null)
                    callback(message);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
