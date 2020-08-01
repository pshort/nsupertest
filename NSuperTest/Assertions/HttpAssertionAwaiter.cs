using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NSuperTest.Assertions
{
    public class HttpAssertionAwaiter : INotifyCompletion
    {
        private HttpAssertionAwaiter _assertAwaiter;
        private TaskAwaiter<HttpResponseMessage> _awaiter;
        private Action<HttpResponseMessage> _assert;

        public TaskAwaiter<HttpResponseMessage> Root;

        public HttpAssertionAwaiter(Task<HttpResponseMessage> task, Action<HttpResponseMessage> assert)
        {
            _awaiter = task.GetAwaiter();
            _assert = assert;
            Root = _awaiter;
        }

        public HttpAssertionAwaiter(HttpAssertionAwaiter previous, Action<HttpResponseMessage> assert)
        {
            _assertAwaiter = previous;
            _assert = assert;
            Root = previous.Root;
        }

        public void OnCompleted(Action continuation)
        {
            if (_assertAwaiter != null)
            {
                _assertAwaiter.OnCompleted(continuation);
            }
            else
            {
                _awaiter.OnCompleted(continuation);
            }
        }

        public bool IsCompleted
        {
            get
            {
                if (_assertAwaiter != null)
                {
                    return _assertAwaiter.IsCompleted;
                }
                else
                {
                    return _awaiter.IsCompleted;
                }
            }
        }

        public HttpAssertionAwaiter GetAwaiter()
        {
            return this;
        }

        public HttpResponseMessage GetResult()
        {
            if (_assertAwaiter != null)
            {
                var result = Root.GetResult();
                _assert(result);
                _assertAwaiter.GetResult();
                return result;
            }
            else
            {
                var result = _awaiter.GetResult();
                _assert(result);
                return result;
            }
        }
    }
}
