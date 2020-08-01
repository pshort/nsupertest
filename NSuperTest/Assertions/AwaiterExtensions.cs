using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ResponseAction = System.Action<System.Net.Http.HttpResponseMessage>;

namespace NSuperTest.Assertions
{
    public static class GenAwaiterExtensions
    {
        public static HttpAssertionAwaiter ExpectBody<T>(this HttpAssertionAwaiter awaiter, Action<T> assert)
        {
            var runCallback = new Func<Action<T>, ResponseAction>(act => new ResponseAction(m => m.Run(act)));
            return new HttpAssertionAwaiter(awaiter, runCallback(assert));
        }
    }
    
    public static class AwaiterExtensions
    {
        private static Func<int, ResponseAction> assertCode = code => new ResponseAction(m => m.AssertStatusCode(code));
        private static Func<ResponseAction, ResponseAction> runCallback = act => new ResponseAction(m => m.Run(act));

        public static HttpAssertionAwaiter ExpectResponse(this HttpAssertionAwaiter awaiter, ResponseAction assert)
        { 
            return new HttpAssertionAwaiter(awaiter, runCallback(assert));
        }

        // codes
        public static HttpAssertionAwaiter ExpectStatus(this HttpAssertionAwaiter awaiter, int code) => new HttpAssertionAwaiter(awaiter, assertCode(code));
        public static HttpAssertionAwaiter ExpectStatus(this Task<HttpResponseMessage> task, int code) => new HttpAssertionAwaiter(task, assertCode(code));

        public static HttpAssertionAwaiter ExpectStatus(this HttpAssertionAwaiter awaiter, HttpStatusCode status) => new HttpAssertionAwaiter(awaiter, assertCode((int)status));
        public static HttpAssertionAwaiter ExpectStatus(this Task<HttpResponseMessage> task, HttpStatusCode status) => new HttpAssertionAwaiter(task, assertCode((int)status));
    }
}
