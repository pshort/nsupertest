using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ResponseAction = System.Action<System.Net.Http.HttpResponseMessage>;
using BadRequestAction = System.Action<NSuperTest.Models.ErrorList>;
using NSuperTest.Assertions;

namespace NSuperTest
{
    public static class GenAwaiterExtensions
    {
        public static HttpAssertionAwaiter ExpectBody<T>(this HttpAssertionAwaiter awaiter, Action<T> assert)
        {
            var runCallback = new Func<Action<T>, ResponseAction>(act => new ResponseAction(m => m.Run(act)));
            return new HttpAssertionAwaiter(awaiter, runCallback(assert));
        }

        public static HttpAssertionAwaiter ExpectBody<T>(this Task<HttpResponseMessage> task, Action<T> assert)
        {
            var runCallback = new Func<Action<T>, ResponseAction>(act => new ResponseAction(m => m.Run(act)));
            return new HttpAssertionAwaiter(task, runCallback(assert));
        }

        public static HttpAssertionAwaiter ExpectSchema<T>(this HttpAssertionAwaiter awaiter, bool useCamelCase = true)
        {
            var run = new Func<Action<T>, ResponseAction>(act => new ResponseAction(m => m.AssertResponseSchema<T>(useCamelCase)));
            return new HttpAssertionAwaiter(awaiter, run(t => { return; }));
        }

        public static HttpAssertionAwaiter ExpectSchema<T>(this Task<HttpResponseMessage> task, bool useCamelCase = true)
        {
            var run = new Func<Action<T>, ResponseAction>(act => new ResponseAction(m => m.AssertResponseSchema<T>(useCamelCase)));
            return new HttpAssertionAwaiter(task, run(t => { return; }));
        }
    }
    
    public static class AwaiterExtensions
    {
        private static Func<int, ResponseAction> assertCode = code => new ResponseAction(m => m.AssertStatusCode(code));

        private static Func<ResponseAction, ResponseAction> runCallback = act => new ResponseAction(m => m.Run(act));

        private static Func<string, string, ResponseAction> assertHeader = (name, value) => new ResponseAction(m => m.AssertHeader(name, value));

        private static Func<BadRequestAction, ResponseAction> assertBadRequest = act => new ResponseAction(m => m.AssertBadRequest(act));

        private static Func<string, ResponseAction> assertSchema = schema =>  new ResponseAction(async m => await m.AssertResponseSchema(schema));

        private static Func<string, ResponseAction> assertBodyString = body => 
            new ResponseAction(m => m.AssertBody(body));
        private static Func<object, bool, ResponseAction> assertBodyObject = (body, useCamelCase) => 
            new ResponseAction(m => m.AssertBody(body, useCamelCase));

        public static HttpAssertionAwaiter ExpectResponse(this HttpAssertionAwaiter awaiter, ResponseAction assert) => 
            new HttpAssertionAwaiter(awaiter, runCallback(assert));
        public static HttpAssertionAwaiter ExpectResponse(this Task<HttpResponseMessage> task, ResponseAction assert) => 
            new HttpAssertionAwaiter(task, runCallback(assert));

        // codes
        public static HttpAssertionAwaiter ExpectStatus(this HttpAssertionAwaiter awaiter, int code) => 
            new HttpAssertionAwaiter(awaiter, assertCode(code));
        public static HttpAssertionAwaiter ExpectStatus(this Task<HttpResponseMessage> task, int code) => 
            new HttpAssertionAwaiter(task, assertCode(code));

        public static HttpAssertionAwaiter ExpectStatus(this HttpAssertionAwaiter awaiter, HttpStatusCode status) => 
            new HttpAssertionAwaiter(awaiter, assertCode((int)status));
        public static HttpAssertionAwaiter ExpectStatus(this Task<HttpResponseMessage> task, HttpStatusCode status) => 
            new HttpAssertionAwaiter(task, assertCode((int)status));


        public static HttpAssertionAwaiter ExpectBadRequest(this HttpAssertionAwaiter awaiter, BadRequestAction assert) =>
            new HttpAssertionAwaiter(awaiter, assertBadRequest(assert));
        public static HttpAssertionAwaiter ExpectBadRequest(this Task<HttpResponseMessage> task, BadRequestAction assert) =>
            new HttpAssertionAwaiter(task, assertBadRequest(assert));
        
        // headers
        public static HttpAssertionAwaiter ExpectHeader(this HttpAssertionAwaiter awaiter, string name, string value) => 
            new HttpAssertionAwaiter(awaiter, assertHeader(name, value));
        public static HttpAssertionAwaiter ExpectHeader(this Task<HttpResponseMessage> task, string name, string value) => 
            new HttpAssertionAwaiter(task, assertHeader(name, value));

        // string body
        public static HttpAssertionAwaiter ExpectBody(this HttpAssertionAwaiter awaiter, string body) => 
            new HttpAssertionAwaiter(awaiter, assertBodyString(body));
        public static HttpAssertionAwaiter ExpectBody(this Task<HttpResponseMessage> task, string body) => 
            new HttpAssertionAwaiter(task, assertBodyString(body));

        // object body
        public static HttpAssertionAwaiter ExpectBody(this HttpAssertionAwaiter awaiter, object body, bool useCamelCase = true) => 
            new HttpAssertionAwaiter(awaiter, assertBodyObject(body, useCamelCase));
        public static HttpAssertionAwaiter ExpectBody(this Task<HttpResponseMessage> task, object body, bool useCamelCase = true) => 
            new HttpAssertionAwaiter(task, assertBodyObject(body, useCamelCase));

        public static HttpAssertionAwaiter ExpectSchema(this HttpAssertionAwaiter awaiter, string schema) =>
            new HttpAssertionAwaiter(awaiter, assertSchema(schema));
        public static HttpAssertionAwaiter ExpectSchema(this Task<HttpResponseMessage> task, string schema) =>
            new HttpAssertionAwaiter(task, assertSchema(schema));
    }
}
